#region

using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class InsertStatement : InsertDeleteStatementBase
    {
        internal InsertStatement()
        {
            Columns = new List<string>();
            Values = new List<List<string>>();
        }

        public string TableName { get; set; }
        public bool HasInto { get; set; }
        public List<string> Columns { get; set; }
        public List<List<string>> Values { get; set; }
        public SelectStatement SourceStatement { get; set; }
    }
}