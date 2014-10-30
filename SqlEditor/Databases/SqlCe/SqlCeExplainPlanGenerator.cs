using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeExplainPlanGenerator : ExplainPlanGenerator
    {
        public override ExplainPlanData GetExplainPlan([NotNull] DatabaseConnection databaseConnection,
            [NotNull] string sql)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (sql == null) throw new ArgumentNullException("sql");
            
            string xml;
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SET SHOWPLAN_XML ON";
                    command.ExecuteNonQuery();

                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        dr.Read();
                    }

                    command.CommandText = "SELECT @@SHOWPLAN";
                    using (var dr = command.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            throw new Exception("Could not obtain explain plan because database did not return any results");
                        }
                        xml = dr.GetString(0);
                    }
                }
            }

            var table = new DataTable();
            table.Columns.Add("PhysicalOp", typeof (string));
            table.Columns.Add("LogicalOp", typeof(string));
            table.Columns.Add("ObjectName", typeof(string));
            table.Columns.Add("EstimateRows", typeof(double));
            table.Columns.Add("EstimateIO", typeof(double));
            table.Columns.Add("EstimateCPU", typeof(double));
            table.Columns.Add("AvgRowSize", typeof(double));
            table.Columns.Add("EstimatedTotalSubtreeCost", typeof(double));

            // Parse table
            ShowPlanXML showPlan;
            var serializer = new XmlSerializer(typeof(ShowPlanXML));
            using (var stream = new StringReader(xml))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    showPlan = (ShowPlanXML)serializer.Deserialize(reader);
                }
            }

            var explainPlanData = new SqlCeExplainPlanData();


            foreach (var stmtBlockType in showPlan.BatchSequence)
            {
                foreach (var blockType in stmtBlockType)
                {
                    foreach (var stmtInfo in blockType.Items)
                    {
                        ParseStmtInfoType(stmtInfo, explainPlanData.Rows, table);
                    }
                }
            }
            
            if (explainPlanData.Rows.Count == 0)
            {
                throw new Exception("Could not find a starting point in the execution plan.");
            }
            return explainPlanData;
        }

        private void ParseStmtInfoType(BaseStmtInfoType genericStatement, List<ExplainPlanRow> rows, DataTable table)
        {
            var stmtSimpleType = genericStatement as StmtSimpleType;
            if (stmtSimpleType != null)
            {
                var relOp = stmtSimpleType.QueryPlan.RelOp;
                var explainPlanRow = AddExplainPlanRow(rows, table, relOp);
                ParseRelOpBaseType(relOp.Item, explainPlanRow, table);
                return;
            }
            throw new Exception("Invalid StmtInfoType of " + genericStatement.GetType());
        }

        private static void ParseRelOpBaseType(RelOpBaseType relOp, ExplainPlanRow explainPlanRow, DataTable table)
        {
            if (relOp == null) return;

            var nestedLoopOp = relOp as NestedLoopsType;
            if (nestedLoopOp != null)
            {
                foreach (var op1 in nestedLoopOp.RelOp)
                {
                    var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                    if (childExplainPlanRow != null && op1.Item != null)
                    {
                        ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                    }
                }
                return;
            }

            var indexScan = relOp as IndexScanType;
            if (indexScan != null && indexScan.Object.Length > 0)
            {
                explainPlanRow.Row.SetField("ObjectName", indexScan.Object[0].Index);
                return;
            }

            var mergeType = relOp as MergeType;
            if (mergeType != null)
            {
                foreach (var op1 in mergeType.RelOp)
                {
                    var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                    if (childExplainPlanRow != null && op1.Item != null)
                    {
                        ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                    }
                }
                return;
            }

            var sortType = relOp as SortType;
            if (sortType != null)
            {
                var op1 = sortType.RelOp;
                var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                if (childExplainPlanRow != null && op1.Item != null)
                {
                    ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                }
                return;
            }

            var computeScalarType = relOp as ComputeScalarType;
            if (computeScalarType != null)
            {
                var op1 = computeScalarType.RelOp;
                var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                if (childExplainPlanRow != null && op1.Item != null)
                {
                    ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                }
                return;
            }

            var hashType = relOp as HashType;
            if (hashType != null)
            {
                foreach (var op1 in hashType.RelOp)
                {
                    var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                    if (childExplainPlanRow != null && op1.Item != null)
                    {
                        ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                    }
                }
                return;
            }

            var updateType = relOp as UpdateType;
            if (updateType != null)
            {
                var op1 = updateType.RelOp;
                var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                if (childExplainPlanRow != null && op1.Item != null)
                {
                    ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                }
                return;
            }

            var simpleUpdateType = relOp as SimpleUpdateType;
            if (simpleUpdateType != null && simpleUpdateType.Object.Length > 0)
            {
                explainPlanRow.Row.SetField("ObjectName", simpleUpdateType.Object[0].Index);
                return;
            }

            var tableScanType = relOp as TableScanType;
            if (tableScanType != null && tableScanType.Object.Length > 0)
            {
                explainPlanRow.Row.SetField("ObjectName", tableScanType.Object[0].Index);
                return;
            }

            var filterType = relOp as FilterType;
            if (filterType != null)
            {
                var op1 = filterType.RelOp;
                var childExplainPlanRow = AddExplainPlanRow(explainPlanRow.ChildRows, table, op1);
                if (childExplainPlanRow != null && op1.Item != null)
                {
                    ParseRelOpBaseType(op1.Item, childExplainPlanRow, table);
                }

                var predicate = filterType.Predicate;
                if (predicate != null && predicate.ScalarOperator != null)
                {
                    ParseScalarType(explainPlanRow, table, predicate.ScalarOperator.Item);
                }
                return;
            }

            /*
             * [System.Xml.Serialization.XmlIncludeAttribute(typeof(NestedLoopsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MergeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConcatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CollapseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BitmapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SortType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TopSortType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StreamAggregateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ParallelismType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComputeScalarType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HashType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TableValuedFunctionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RowsetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ScalarInsertType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateIndexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleUpdateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IndexScanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TableScanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstantScanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FilterType))]
             */
        }

        private static void ParseScalarType(ExplainPlanRow row, DataTable table, object item)
        {
            var scalarType1 = item as ScalarType;
            if (scalarType1 != null)
            {
                ParseScalarType(row, table, scalarType1.Item);
                return;
            }

            var logical = item as LogicalType;
            if (logical != null && logical.ScalarOperator != null)
            {
                foreach (var scalarType in logical.ScalarOperator)
                {
                    ParseScalarType(row, table, scalarType);
                }
                return;
            }

            var subqueryType = item as SubqueryType;
            if (subqueryType != null && subqueryType.RelOp != null)
            {
                var childRow = AddExplainPlanRow(row.ChildRows, table,
                    new object[] {"Subquery", "Subquery", null, null, null, null, null});
                ParseRelOpBaseType(subqueryType.RelOp.Item, childRow, table);
                if (subqueryType.ScalarOperator != null)
                {
                    ParseScalarType(childRow, table, subqueryType.ScalarOperator);
                }
                return;
            }

            var aggregateType = item as AggregateType;
            if (aggregateType != null && aggregateType.ScalarOperator != null)
            {
                foreach (var scalarType in aggregateType.ScalarOperator)
                {
                    ParseScalarType(row, table, scalarType);
                }
                return;
            }

            var arithmeticType = item as ArithmeticType;
            if (arithmeticType != null && arithmeticType.ScalarOperator != null)
            {
                foreach (var scalarType in arithmeticType.ScalarOperator)
                {
                    ParseScalarType(row, table, scalarType);
                }
                return;
            }

            var assignType = item as AssignType;
            if (assignType != null)
            {
                foreach (var scalarType in new [] { assignType.Item as ScalarType, assignType.ScalarOperator }.Where(x => x != null).ToList())
                {
                    ParseScalarType(row, table, scalarType);
                }
                return;
            }

            var compareType = item as CompareType;
            if (compareType != null)
            {
                foreach (var scalarType in new[] { assignType.Item as ScalarType, assignType.ScalarOperator }.Where(x => x != null).ToList())
                {
                    ParseScalarType(row, table, scalarType);
                }
                return;
            }

            var convertType = item as ConvertType;
            if (convertType != null && convertType.ScalarOperator != null)
            {
                ParseScalarType(row, table, convertType.ScalarOperator);
                return;
            }

            /*
                        * [System.Xml.Serialization.XmlElementAttribute("Aggregate", typeof(AggregateType))]
                           [System.Xml.Serialization.XmlElementAttribute("Arithmetic", typeof(ArithmeticType))]
                           [System.Xml.Serialization.XmlElementAttribute("Assign", typeof(AssignType))]
                           [System.Xml.Serialization.XmlElementAttribute("Compare", typeof(CompareType))]
                           [System.Xml.Serialization.XmlElementAttribute("Const", typeof(ConstType))]
                           [System.Xml.Serialization.XmlElementAttribute("Convert", typeof(ConvertType))]
                           [System.Xml.Serialization.XmlElementAttribute("IF", typeof(ConditionalType))]
                           [System.Xml.Serialization.XmlElementAttribute("Identifier", typeof(IdentType))]
                           [System.Xml.Serialization.XmlElementAttribute("Intrinsic", typeof(IntrinsicType))]
                           [System.Xml.Serialization.XmlElementAttribute("Logical", typeof(LogicalType))]
                           [System.Xml.Serialization.XmlElementAttribute("MultipleAssign", typeof(MultAssignType))]
                           [System.Xml.Serialization.XmlElementAttribute("ScalarExpressionList", typeof(ScalarExpressionListType))]
                           [System.Xml.Serialization.XmlElementAttribute("Sequence", typeof(ScalarSequenceType))]
                           [System.Xml.Serialization.XmlElementAttribute("Subquery", typeof(SubqueryType))]
                           [System.Xml.Serialization.XmlElementAttribute("UDTMethod", typeof(UDTMethodType))]
                           [System.Xml.Serialization.XmlElementAttribute("UserDefinedAggregate", typeof(UDAggregateType))]
                           [System.Xml.Serialization.XmlElementAttribute("UserDefinedFunction", typeof(UDFType))]
                        */

                    
        }

        private static ExplainPlanRow AddExplainPlanRow(ICollection<ExplainPlanRow> childRows, DataTable table, object[] relOp)
        {
            var row = table.NewRow();
            row.ItemArray = relOp;
            var explainPlanRow = new ExplainPlanRow(row);
            childRows.Add(explainPlanRow);
            return explainPlanRow;
        }

        private static ExplainPlanRow AddExplainPlanRow(ICollection<ExplainPlanRow> rows, DataTable table, RelOpType relOp)
        {
            return AddExplainPlanRow(rows, table, new object[]
                                           {
                                               relOp.PhysicalOp.ToString(), relOp.LogicalOp.ToString(), string.Empty,
                                               relOp.EstimateRows, relOp.EstimateIO,
                                               relOp.EstimateCPU,
                                               relOp.AvgRowSize, relOp.EstimatedTotalSubtreeCost
                                           });
        }
    }
}