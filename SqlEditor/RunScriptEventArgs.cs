using System.Collections.Generic;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor
{
    public class RunScriptEventArgs
    {
        public List<string> Statements { get; set; }
        public DatabaseConnection DatabaseConnection { get; set; }

        public RunScriptEventArgs(DatabaseConnection databaseConnection)
        {
            DatabaseConnection = databaseConnection;
            Statements = new List<string>();
        }

        public RunScriptEventArgs(IEnumerable<string> statements, DatabaseConnection databaseConnection)
            : this(databaseConnection)
        {
            Statements.AddRange(statements);
        }
    }
}