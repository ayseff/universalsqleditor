#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    [DebuggerDisplay("{Value}")]
    public class CreateTableStatement : IStatement
    {
        public CreateTableStatement()
        {
            Fields = new FieldDefinitions();
        }

        public string TableName { get; set; }
        public FieldDefinitions Fields { get; set; }

        #region IStatement Members

        public bool Terminated { get; set; }

        public bool ParsedCompletely { get; set; }

        public string Identifier
        {
            get
            {
                return String.Format(
                    "CREATE TABLE {0}\n(\n{1}\n)",
                    TableName,
                    String.Join(",\n", Fields.Select(fld => "\t" + fld.ToString()).ToArray())
                    );
            }
        }

        private readonly List<Table> _tables = new List<Table>();
        public List<Table> Tables
        {
            get { return _tables; }
        }

        #endregion
    }
}