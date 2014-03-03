using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;

namespace SqlEditor.SqlParser.Entities
{
    public class Statement : IStatement
    {
        #region IStatement Members

        public bool Terminated { get; set; }

        public bool ParsedCompletely { get; set; }

        public virtual string Identifier
        {
            get { return ""; }
        }

        private readonly List<Table> _tables = new List<Table>();
        public List<Table> Tables
        {
            get { return _tables; }
        }

        #endregion
    }
}