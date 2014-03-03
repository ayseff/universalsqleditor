#region

using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public enum IndexWithOption
    {
        PadIndex,
        SortInTempDb,
        IgnoreDupKey,
        StatisticsNorecompute,
        DropExisting,
        Online,
        AllowRowLocks,
        AllowPageLocks
    }

    public class RelationalIndexOption
    {
        public IndexWithOption Option { get; set; }

        public Expression Assignment { get; set; }
    }

    public class CreateIndexStatement : CustomStatement
    {
        public CreateIndexStatement()
        {
            Unique = false;
            Clustered = false;
            Columns = new List<IndexedColumn>();
            RelationalIndexOptions = new List<RelationalIndexOption>();
        }

        public bool Clustered { get; set; }
        public bool Unique { get; set; }
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public List<IndexedColumn> Columns { get; set; }
        public List<RelationalIndexOption> RelationalIndexOptions { get; set; }
        public string FileGroupName { get; set; }

        public override string Identifier
        {
            get
            {
                return String.Format(
                    "CREATE INDEX {0}ON {1}({2})",
                    IndexName,
                    TableName,
                    String.Join(", ", Columns.Select(c => c.Name).ToArray())
                    );
            }
        }
    }
}