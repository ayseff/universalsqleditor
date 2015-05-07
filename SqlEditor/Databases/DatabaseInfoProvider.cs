using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using Utilities.Text;
using log4net;

namespace SqlEditor.Databases
{
    public abstract class DbInfoProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected virtual void BeforeRunQuery(IDbConnection connection, string schemaName)
        {}

        protected virtual void AfterRunQuery(IDbConnection connection, string schemaName)
        {}

        public abstract IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection);
        protected virtual IList<DatabaseInstance> GetDatabaseInstancesBase(IDbConnection connection, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.Debug("Getting databases ...");
            var databaseInstances = new List<DatabaseInstance>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        databaseInstances.Add(new DatabaseInstance(dr.GetString(0)));
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} database(s).", databaseInstances.Count);
            return databaseInstances;
        }

        public abstract IList<Schema> GetSchemas(IDbConnection connection, string databaseInstanceName = null);
        protected virtual IList<Schema> GetSchemasBase(IDbConnection connection, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.Debug("Getting schemas ...");
            var schemas = new List<Schema>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        schemas.Add(new Schema(dr.GetString(0)));
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} schema(s).", schemas.Count);
            return schemas;
        }

        public abstract IList<Table> GetTables(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Table> GetTablesBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");
     
            _log.DebugFormat("Getting tables for schema {0} ...", schemaName);
            var schema = new Schema(schemaName);
            var tables = new List<Table>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var table = new Table(dr.GetString(0), schema);
                        if (dr.FieldCount == 2)
                        {
                            table.Id = dr[1];
                        }
                        tables.Add(table);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0} table(s).", tables.Count.ToString("#,0"));
            return tables;
        }

        public abstract IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null);
        public Task<IList<Column>> GetTableColumnsAsync(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName)
        {
            var task = Task.Run(() => GetTableColumns(connection, schemaName, tableName, databaseInstanceName));
            return task;
        }

        public abstract IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null);
        public Task<IList<Column>> GetTablePrimaryKeyColumnsAsync(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            var task = Task.Run(() => GetTablePrimaryKeyColumns(connection, schemaName, tableName, databaseInstanceName));
            return task;
        }

        protected virtual IList<Column> GetTableColumnsBase([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                     [NotNull] string tableName, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            if (sql == null) throw new ArgumentNullException("sql");
            
            var table = new Table(tableName, new Schema(schemaName));
            return  GetObjectColumns(connection, schemaName, table, sql, parameters);
        }

        public abstract IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null);
        protected virtual IList<Partition> GetTablePartitionsBase(IDbConnection connection, string schemaName, string tableName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting partitions for table {0} in schema {1} ...", tableName, schemaName);
            var schema = new Schema(schemaName);
            var table = new Table(tableName, schema);
            var partitions = new List<Partition>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var partition = new Partition(dr.GetString(0).Trim().ToUpper(), table);
                        partitions.Add(partition);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0} partition(s).", partitions.Count.ToString("#,0"));
            return partitions;
        }

        public abstract IList<View> GetViews(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<View> GetViewsBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting views for schema {0} ...", schemaName);
            var schema = new Schema(schemaName);
            var views = new List<View>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var view = new View(dr.GetString(0).Trim().ToUpper(), schema);
                        if (dr.FieldCount == 2)
                        {
                            view.Id = dr[1];
                        }
                        views.Add(view);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} view(s).", views.Count.ToString("#,0"));
            return views;
        }

        public abstract IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName, string databaseInstanceName = null);

        public Task<IList<Column>> GetViewColumnsAsync(IDbConnection connection, string schemaName, string viewName, string databaseInstanceName)
        {
            var task = Task.Run(() => GetViewColumns(connection, schemaName, viewName, databaseInstanceName));
            return task;
        }
        protected virtual IList<Column> GetViewColumnsBase([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                     [NotNull] string viewName, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");
            if (sql == null) throw new ArgumentNullException("sql");
            
            var view = new View(viewName, new Schema(schemaName));
            return GetObjectColumns(connection, schemaName, view, sql, parameters);
        }

        public abstract IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<MaterializedView> GetMaterializedViewsBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting materialized views for schema {0} ...", schemaName);
            var schema = new Schema(schemaName);
            var views = new List<MaterializedView>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var view = new MaterializedView(dr.GetString(0).Trim().ToUpper(), schema);
                        if (dr.FieldCount == 2)
                        {
                            view.Id = dr[1];
                        }
                        views.Add(view);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0} materialized view(s).", views.Count.ToString("#,0"));
            return views;
        }

        public abstract IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName,
                                                                 string materializedViewName, string databaseInstanceName = null);
        protected virtual IList<Column> GetMaterializedViewColumnsBase([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                     [NotNull] string mViewName, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (mViewName == null) throw new ArgumentNullException("mViewName");
            if (sql == null) throw new ArgumentNullException("sql");
            
            var materializedView = new MaterializedView(mViewName, new Schema(schemaName));
            return GetObjectColumns(connection, schemaName, materializedView, sql, parameters);
        }


        public abstract IList<Index> GetIndexes(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Index> GetIndexesBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting indexes for schema {0} ...", schemaName);
            DatabaseInstance databaseInstance = null;
            Schema tableSchema = null;
            Schema indexSchema = null;
            Table table = null;
            var indices = new List<Index>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var dbInstanceNameQuery = dr.IsDBNull(0) ? null : dr.GetString(0).Trim().ToUpper();
                        if (dbInstanceNameQuery == null)
                        {
                            databaseInstance = null;
                        }
                        else if (databaseInstance == null || databaseInstance.Name != dbInstanceNameQuery)
                        {
                            databaseInstance = new DatabaseInstance(dbInstanceNameQuery);
                        }

                        var tableSchemaNameQuery = dr.IsDBNull(1) ? null : dr.GetString(1).Trim().ToUpper();
                        if (tableSchemaNameQuery == null)
                        {
                            tableSchema = null;
                        }
                        else if (tableSchema == null || tableSchema.Name != tableSchemaNameQuery || tableSchema.Parent != databaseInstance)
                        {
                            tableSchema = new Schema(tableSchemaNameQuery) { Parent = databaseInstance};
                        }

                        var tableName = dr.GetString(2).Trim().ToUpper();
                        if (table == null || table.Name != tableName || table.Parent != tableSchema)
                        {
                            table = new Table(tableName, tableSchema);
                        }

                        var indexSchemaNameQuery = dr.IsDBNull(3) ? null : dr.GetString(3).Trim().ToUpper();
                        if (indexSchemaNameQuery == null)
                        {
                            indexSchema = null;
                        }
                        else if (indexSchema == null || indexSchema.Name != indexSchemaNameQuery || indexSchema.Parent != databaseInstance)
                        {
                            indexSchema = new Schema(indexSchemaNameQuery) { Parent = databaseInstance };
                        }

                        var indexName = dr.GetString(4).Trim().ToUpper();
                        var isUnqiueString = dr.IsDBNull(5) ? "0" : dr[5].ToString().Trim().ToUpper();
                        var index = new Index(indexName, indexSchema);
                        index.Table = table;
                        index.IsUnique = isUnqiueString == "1" || isUnqiueString == "Y" || isUnqiueString == "YES" ||
                                         isUnqiueString == "U" || isUnqiueString == "P";
                        if (dr.FieldCount == 7)
                        {
                            index.Id = dr[6];
                        }
                        indices.Add(index);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0} index(es).", indices.Count.ToString("#,0"));
            return indices;
        }

        public abstract IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null);

        public abstract IList<Column> GetIndexColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, string indexName, object indexId = null, string databaseInstanceName = null);

        public abstract IList<Column> GetIndexIncludedColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, string indexName, object indexId = null, string databaseInstanceName = null);
        protected virtual IList<Column> GetIndexColumnsBase([NotNull] IDbConnection connection,
                                                            [NotNull] string schemaName, [NotNull] string indexName,
                                                            [NotNull] string sql,
                                                            params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            if (sql == null) throw new ArgumentNullException("sql");
            var schema = new Schema(schemaName);
            return GetObjectColumns(connection, schemaName, new Index(indexName, schema), sql, parameters);
        }

        public abstract IList<Sequence> GetSequences(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Sequence> GetSequencesBase(IDbConnection connection, string schemaName,
                                                           [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting sequences for schema {0} ...", schemaName);
            var schema = new Schema(schemaName);
            var sequences = new List<Sequence>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr.IsDBNull(0))
                        {
                            throw new Exception("Sequence name cannot be null");
                        }
                        // iss.sequence_name, iss.minimum_value, iss.maximum_value, iss.increment, sq.current_value
                        var sequence = new Sequence(dr.GetString(0), schema);
                        sequence.MinimumValue = decimal.Parse(dr[1].ToString());
                        sequence.MaximumValue = decimal.Parse(dr[2].ToString());
                        sequence.Increment = decimal.Parse(dr[3].ToString());
                        sequence.CurrentValue = decimal.Parse(dr[4].ToString());
                        if (dr.FieldCount == 6)
                        {
                            sequence.Id = dr[5];
                        }
                        sequences.Add(sequence);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} sequence(s).", sequences.Count.ToString("#,0"));
            return sequences;
        }

        public abstract IList<Constraint> GetConstraints(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Constraint> GetConstraintsBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting constraints for schema {0} ...", schemaName);
            Schema schema = null;
            var constraints = new List<Constraint>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var schemaNameQuery = dr.GetString(0).Trim().ToUpper();
                        if (schema == null || schema.Name != schemaNameQuery)
                        {
                            schema = new Schema(schemaNameQuery);
                        }
                        var constraint = new Constraint(dr.GetString(1).Trim().ToUpper(), schema);
                        var isEnforcedString = dr.IsDBNull(2) ? "0" : dr[2].ToString().Trim().ToUpper();
                        constraint.IsEnforced = isEnforcedString == "1" || isEnforcedString == "Y" ||
                                                isEnforcedString == "YES";
                        constraint.Type = dr.GetString(3);
                        if (dr.FieldCount == 5)
                        {
                            constraint.Id = dr[4];
                        }
                        constraints.Add(constraint);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0} constraint(s).", constraints.Count.ToString("#,0"));
            return constraints;
        }

        public abstract IList<Constraint> GetConstraintsForTable(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null);

        public abstract IList<Trigger> GetTriggers(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Trigger> GetTriggersBase(IDbConnection connection, string schemaName, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting triggers for schema {0} ...", schemaName);
            var schema = new Schema(schemaName);
            var triggers = new List<Trigger>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var trigger = new Trigger(dr.GetString(0).Trim().ToUpper(), schema);
                        if (dr.FieldCount == 2)
                        {
                            trigger.Id = dr[1];
                        }
                        triggers.Add(trigger);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0} trigger(s).", triggers.Count.ToString("#,0"));
            return triggers;
        }

        public abstract IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        public abstract IList<Synonym> GetSynonyms(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Synonym> GetSynonymsBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting synonyms for schema {0} ...", schemaName);
            BeforeRunQuery(connection, schemaName);
            var schema = new Schema(schemaName);
            var synonyms = new List<Synonym>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var synonym = new Synonym(dr.GetString(0).Trim().ToUpper(), schema);
                        synonym.TargetObjectName = dr.GetString(1);
                        if (dr.FieldCount == 3)
                        {
                            synonym.Id = dr[2];
                        }
                        synonyms.Add(synonym);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} synonyms.", synonyms.Count.ToString("#,0"));
            return synonyms;
        }

        public abstract IList<StoredProcedure> GetStoredProcedures(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<StoredProcedure> GetStoredProceduresBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting stored procedures for schema {0} ...", schemaName);
            BeforeRunQuery(connection, schemaName);
            var schema = new Schema(schemaName);
            var storedProcedures = new List<StoredProcedure>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var storedProcedure = new StoredProcedure(dr.GetString(1).Trim().ToUpper(), schema);
                        storedProcedure.ObjectId = dr[0].ToString();
                        storedProcedure.Definition = dr.IsDBNull(2) ? null : dr.GetString(2);
                        if (dr.FieldCount == 4)
                        {
                            storedProcedure.Id = dr[3];
                        }
                        storedProcedures.Add(storedProcedure);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} stored procedure(s).", storedProcedures.Count.ToString("#,0"));
            return storedProcedures;
        }

        public abstract IList<Function> GetFunctions(IDbConnection connection, string schemaName, string databaseInstanceName = null);
        protected virtual IList<Function> GetFunctionsBase(IDbConnection connection, string schemaName,
                                                         [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting functions for schema {0} ...", schemaName);
            BeforeRunQuery(connection, schemaName);
            var schema = new Schema(schemaName);
            var functions = new List<Function>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var function = new Function(dr.GetString(1).Trim().ToUpper(), schema);
                        function.ObjectId = dr[0].ToString();
                        function.Definition = dr.IsDBNull(2) ? null : dr.GetString(2);
                        if (dr.FieldCount == 4)
                        {
                            function.Id = dr[3];
                        }
                        functions.Add(function);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} function(s).", functions.Count.ToString("#,0"));
            return functions;
        }




        public abstract IList<ColumnParameter> GetStoredProcedureParameters([NotNull] IDbConnection connection,
                                                                           [NotNull] StoredProcedure storedProcedure);
        protected virtual IList<ColumnParameter> GetStoredProcedureParametersBase([NotNull] IDbConnection connection,
                                                                                  [NotNull] StoredProcedure storedProcedure,
                                                                            [NotNull] string sql, 
                                                                                params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");
            if (sql == null) throw new ArgumentNullException("sql");

            var schemaName = storedProcedure.Parent.Name;
            _log.DebugFormat("Getting stored procedure parameters for store procedure {0} ...", storedProcedure.FullyQualifiedName);
            BeforeRunQuery(connection, schemaName);
            var storedProcedureParameters = new List<ColumnParameter>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var parameter = GetParameter(dr, storedProcedure);
                        parameter.Parent = storedProcedure;
                        storedProcedureParameters.Add(parameter);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} stored procedure parameter(s).", storedProcedureParameters.Count.ToString("#,0"));
            return storedProcedureParameters;
        }

        public abstract IList<ColumnParameter> GetFunctionParameters([NotNull] IDbConnection connection,
                                                                           [NotNull] Function function);

        public abstract IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function);

        public abstract IList<Package> GetPackages(IDbConnection connection, string schemaName, string databaseInstanceName = null);

        public abstract IList<PackageProcedure> GetPackageProcedures(IDbConnection connection, string schemaName, string packageName, string databaseInstanceName = null);


        public abstract IList<Login> GetLogins(IDbConnection connection, string databaseInstanceName = null);
        protected virtual IList<Login> GetLoginsBase(IDbConnection connection, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.Debug("Getting logins ...");
            var logins = new List<Login>();
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        logins.Add(new Login(dr.GetString(0)));
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} login(s).", logins.Count);
            return logins;
        }

        public abstract IntelisenseData GetIntelisenseData(IDbConnection connection, string currentSchemaName);

        protected IList<Column> GetTableColumnsDefault<T>(IDbConnection connection,
                                                          DatabaseObjectWithColumns databaseObjectWithColumns)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (databaseObjectWithColumns == null) throw new ArgumentNullException("databaseObjectWithColumns");
            if (databaseObjectWithColumns.Name.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("databaseObjectWithColumns", "Object name is null or empty");

            var columns = new List<Column>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT * FROM {0}", databaseObjectWithColumns.FullyQualifiedName);
                using (var dr = command.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    var schemaTable = dr.GetSchemaTable();
                    var enums = typeof(T).GetEnumValues();
                    if (schemaTable != null)
                    {
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            Type dataType = null;
                            var columnName = string.Empty;
                            var providerType = 0;
                            var columnOrdinal = 0;
                            int? columnSize = null, dataPrecision = null, dataScale = null;
                            var nullable = false;

                            if (!row.IsNull("ColumnName"))
                            {
                                columnName = (String)row["ColumnName"];
                            }
                            if (!row.IsNull("DataType"))
                            {
                                dataType = (Type)row["DataType"];
                            }
                            if (!row.IsNull("ProviderType"))
                            {
                                providerType = (int)row["ProviderType"];
                            }
                            if (!row.IsNull("ColumnOrdinal"))
                            {
                                columnOrdinal = (int)row["ColumnOrdinal"];
                            }
                            if (!row.IsNull("ColumnSize"))
                            {
                                columnSize = (int)row["ColumnSize"];
                            }
                            if (!row.IsNull("NumericPrecision"))
                            {
                                dataPrecision = (short)row["NumericPrecision"];
                            }
                            if (!row.IsNull("NumericScale"))
                            {
                                dataScale = (short)row["NumericScale"];
                            }
                            if (!row.IsNull("AllowDBNull"))
                            {
                                nullable = (bool)row["AllowDBNull"];
                            }

                            var dataTypeString =
                                enums.Cast<object>()
                                     .Where(val => (int)val == providerType)
                                     .Select(val => val.ToString())
                                     .
                                      FirstOrDefault();

                            var column = new Column(columnName.Trim().ToUpper(), databaseObjectWithColumns)
                            {
                                DataType = dataTypeString,
                                OrdinalPosition = columnOrdinal,
                                CharacterLength = columnSize,
                                Nullable = nullable
                            };
                            if (dataType != null &&
                                (dataType == typeof(Int16) || dataType == typeof(Int32) ||
                                 dataType == typeof(Int64) ||
                                 dataType == typeof(Double) || dataType == typeof(Decimal) ||
                                 dataType == typeof(float)))
                            {
                                column.DataPrecision = dataPrecision;
                            }
                            if (dataType != null &&
                                (dataType == typeof(Double) || dataType == typeof(Decimal) ||
                                 dataType == typeof(float)))
                            {
                                column.DataScale = dataScale;
                            }
                            columns.Add(column);
                        }
                    }
                }
            }
            return columns;
        }


        protected IntelisenseData GetIntellisenseDataHelper(IDbConnection connection, string currentSchemaName,
                                                            string tablesSql, string viewsSql, string mviewsSql)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            var intellisenseData = new IntelisenseData();
            using (var command = connection.CreateCommand())
            {
                // Load tables
                if (!tablesSql.IsNullEmptyOrWhitespace())
                {
                    command.CommandText = tablesSql;
                    using (var dr = command.ExecuteReader())
                    {
                        string previousSchemaName = null, previousTableName = null;
                        Schema schema = null;
                        Table table = null;
                        while (dr.Read())
                        {
                            var schemaName = dr.GetString(0).Trim();
                            if (schemaName != previousSchemaName)
                            {
                                previousSchemaName = schemaName;
                                schema = intellisenseData.AllSchemas.FirstOrDefault(s => string.Equals(s.Name, schemaName, StringComparison.CurrentCultureIgnoreCase));
                                if (schema == null)
                                {
                                    schema = new Schema(schemaName);
                                    intellisenseData.AllSchemas.Add(schema);
                                }
                            }

                            var objectName = dr.GetString(1).Trim();
                            if (objectName != previousTableName || schemaName != previousSchemaName)
                            {
                                previousTableName = objectName;
                                table =
                                    schema.Tables.FirstOrDefault(
                                        o => o.Name == objectName && o.Parent != null && o.Parent.Name == schemaName);
                                if (table == null)
                                {
                                    table = new Table(objectName, schema);
                                    schema.Tables.Add(table);
                                    intellisenseData.AllObjects.Add(table);
                                }
                            }

                            var columnName = dr.GetString(2).Trim();
                            var column = new Column(columnName, table);
                            table.Columns.Add(column);
                            intellisenseData.AllColumns.Add(column);
                        }
                    }
                }

                // Load views
                if (!viewsSql.IsNullEmptyOrWhitespace())
                {
                    command.CommandText = viewsSql;
                    using (IDataReader dr = command.ExecuteReader())
                    {
                        string previousSchemaName = null, previousViewName = null;
                        Schema schema = null;
                        View view = null;
                        while (dr.Read())
                        {
                            string schemaName = dr.GetString(0).Trim();
                            if (schemaName != previousSchemaName)
                            {
                                previousSchemaName = schemaName;
                                schema = intellisenseData.AllSchemas.FirstOrDefault(s => string.Equals(s.Name, schemaName, StringComparison.CurrentCultureIgnoreCase));
                                if (schema == null)
                                {
                                    schema = new Schema(schemaName);
                                    intellisenseData.AllSchemas.Add(schema);
                                }
                            }

                            var objectName = dr.GetString(1).Trim();
                            if (objectName != previousViewName || schemaName != previousSchemaName)
                            {
                                previousViewName = objectName;
                                view =
                                    schema.Views.FirstOrDefault(
                                        o => o.Name == objectName && o.Parent != null && o.Parent.Name == schemaName);
                                if (view == null)
                                {
                                    view = new View(objectName, schema);
                                    schema.Views.Add(view);
                                    intellisenseData.AllObjects.Add(view);
                                }
                            }

                            var columnName = dr.GetString(2).Trim();
                            var column = new Column(columnName, view);
                            view.Columns.Add(column);
                            intellisenseData.AllColumns.Add(column);
                        }
                    }
                }

                // Load MViews
                if (!mviewsSql.IsNullEmptyOrWhitespace())
                {
                    command.CommandText = mviewsSql;
                    using (var dr = command.ExecuteReader())
                    {
                        string previousSchemaName = null, previousViewName = null;
                        Schema schema = null;
                        MaterializedView materializedView = null;
                        while (dr.Read())
                        {
                            var schemaName = dr.GetString(0).Trim();
                            if (schemaName != previousSchemaName)
                            {
                                previousSchemaName = schemaName;
                                schema = intellisenseData.AllSchemas.FirstOrDefault(s => string.Equals(s.Name, schemaName, StringComparison.CurrentCultureIgnoreCase));
                                if (schema == null)
                                {
                                    schema = new Schema(schemaName);
                                    intellisenseData.AllSchemas.Add(schema);
                                }
                            }

                            var objectName = dr.GetString(1).Trim();
                            if (objectName != previousViewName || schemaName != previousSchemaName)
                            {
                                previousViewName = objectName;
                                materializedView =
                                    schema.MaterializedViews.FirstOrDefault(
                                        o => o.Name == objectName && o.Parent != null && o.Parent.Name == schemaName);
                                if (materializedView == null)
                                {
                                    materializedView = new MaterializedView(objectName, schema);
                                    schema.MaterializedViews.Add(materializedView);
                                    intellisenseData.AllObjects.Add(materializedView);
                                }
                            }

                            var columnName = dr.GetString(2).Trim();
                            var column = new Column(columnName, materializedView);
                            materializedView.Columns.Add(column);
                            intellisenseData.AllColumns.Add(column);
                        }
                    }
                }
            }

            // Set current schema
            intellisenseData.CurrentSchema =
                intellisenseData.AllSchemas.FirstOrDefault(x => string.Equals(x.Name, currentSchemaName.Trim(), StringComparison.CurrentCultureIgnoreCase));
            if (intellisenseData.CurrentSchema == null && intellisenseData.AllSchemas.Count > 0)
            {
                intellisenseData.CurrentSchema = intellisenseData.AllSchemas.First();
            }
            return intellisenseData;
        }

        protected IList<Column> GetObjectColumns([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                     [NotNull] DatabaseObjectWithColumns databaseObject, [NotNull] string sql, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseObject == null) throw new ArgumentNullException("databaseObject");
            if (sql == null) throw new ArgumentNullException("sql");

            _log.DebugFormat("Getting columns for schema {0} and object {1} ...", schemaName, databaseObject.Name);
            var columns = new List<Column>();
            BeforeRunQuery(connection, schemaName);
            using (var command = connection.CreateCommand())
            {
                BuildSqlCommand(command, sql, parameters);
                using (var dr = command.ExecuteReader())
                {
                    // column_name, data_type, data_length, data_precision, data_scale, nullable, column_id
                    // column_name, c.DATA_TYPE, c.DATA_LENGTH, C.DATA_PRECISION, C.NULLABLE, C.COLUMN_ID
                    while (dr.Read())
                    {
                        var column = GetColumn(dr, databaseObject);
                        databaseObject.Columns.Add(column);
                        columns.Add(column);
                    }
                }
            }
            AfterRunQuery(connection, schemaName);
            _log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            return columns;
        }

        private static Column GetColumn(IDataReader dr, DatabaseObjectWithColumns @object)
        {
            if (dr.IsDBNull(0))
            {
                throw new Exception("Column name cannot be null");
            }

            var column = new Column(dr.GetString(0), @object);
            FillColumn(dr, column);
            @object.Columns.Add(column);
            return column;
        }

        private static void FillColumn(IDataReader dr, Column column)
        {
            column.DataType = dr.IsDBNull(1) ? null : dr.GetString(1);
            column.CharacterLength = dr.IsDBNull(2) ? (long?) null : long.Parse(dr[2].ToString());
            column.DataPrecision = dr.IsDBNull(3) ? (int?) null : int.Parse(dr[3].ToString());
            column.DataScale = dr.IsDBNull(4) ? (int?) null : int.Parse(dr[4].ToString());
            var isNullable = dr[5].ToString().ToUpper().Trim();
            column.Nullable = isNullable == "Y" || isNullable == "YES" || isNullable == "1";
            column.OrdinalPosition = int.Parse(dr[6].ToString());
        }

        private static ColumnParameter GetParameter(IDataReader dr, DatabaseObjectWithColumns @object)
        {
            if (dr.IsDBNull(0))
            {
                throw new Exception("Parameter name cannot be null");
            }

            var parameter = new ColumnParameter(dr.GetString(0), @object);
            FillColumn(dr, parameter);
            parameter.Direction = dr.IsDBNull(7) ? null : dr.GetString(7);
            return parameter;
        }

        protected static void BuildSqlCommand(IDbCommand command, string sql, object[] parameters)
        {
            var parameterSymbol = "@";
            command.CommandText = sql;

            // Oracle command does not like @ in the parameter names so we have to use ':' instead
            var oracleCommand = command as OracleCommand;
            if (oracleCommand != null)
            {
                parameterSymbol = ":";
                for (var i = 0; i < parameters.Length; i++)
                {
                    command.CommandText = command.CommandText.Replace("@" + (i + 1), ":" + (i + 1));
                }
            }
            
            if (parameters != null && parameters.Length > 0)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var param = command.CreateParameter();
                    param.ParameterName = parameterSymbol + (i + 1);
                    param.Value = parameters[i];
                    command.Parameters.Add(param);
                }
            }
        }

        
    }
}