#region

using System;
using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class CreateViewStatement : IStatement
    {
        public SelectStatement SelectBlock { get; set; }
        public string Name { get; set; }

        #region IStatement Members

        public bool Terminated { get; set; }

        public bool ParsedCompletely { get; set; }

        public string Identifier
        {
            get { return String.Format("CREATE VIEW {0} AS {1}", Name, SelectBlock.Identifier); }
        }

        public List<Table> Tables
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}