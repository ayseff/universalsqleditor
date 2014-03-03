#region

using System.Collections.Generic;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class CustomStatement : Statement
    {
        /// <summary>
        ///   Initializes a new instance of the BaseStatement class.
        /// </summary>
        public CustomStatement()
        {
            From = new List<Table>();
            ParsedCompletely = false;
        }

        public List<Table> From { get; set; }
        public Expression Where { get; set; }
    }
}