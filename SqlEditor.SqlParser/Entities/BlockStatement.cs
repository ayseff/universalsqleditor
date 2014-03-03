#region

using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class BlockStatement : CustomStatement
    {
        /// <summary>
        ///   Initializes a new instance of the BlockStatement class.
        /// </summary>
        public BlockStatement()
        {
            Statements = new List<IStatement>();
        }

        public List<IStatement> Statements { get; set; }
    }
}