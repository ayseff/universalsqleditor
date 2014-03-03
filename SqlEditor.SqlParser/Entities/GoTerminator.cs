using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;

namespace SqlEditor.SqlParser.Entities
{
    public class GoTerminator : IStatement
    {
        #region IStatement Members

        public bool Terminated { get; set; }

        public bool ParsedCompletely
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Identifier
        {
            get { return "GO"; }
        }

        public List<Table> Tables
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}