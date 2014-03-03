#region

using System;
using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class AlterTableStatement : IStatement
    {
        internal AlterTableStatement()
        {
        }

        public List<string> PrimaryKeys { get; set; }
        public string ConstraintName { get; set; }
        public string TableName { get; set; }

        #region IStatement Members

        public bool Terminated { get; set; }

        public bool ParsedCompletely { get; set; }

        public string Identifier
        {
            get
            {
                return String.Format(
                    "ALTER TABLE {0} ADD CONSTRAINT {1} PRIMARY KEY CLUSTERED ({2})",
                    TableName,
                    ConstraintName,
                    String.Join(", ", PrimaryKeys.ToArray())
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