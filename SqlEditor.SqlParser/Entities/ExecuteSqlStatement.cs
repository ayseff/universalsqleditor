#region

using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class Argument
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
    }

    public class ExecuteSqlStatement : Statement
    {
        public ExecuteSqlStatement()
        {
            Arguments = new List<Argument>();
        }

        public string FunctionName { get; set; }
        public List<Argument> Arguments { get; private set; }
        public IStatement InnerStatement { get; set; }
    }
}