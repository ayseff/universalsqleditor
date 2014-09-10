using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases
{
    public abstract class DdlGenerator
    {
        public abstract string GenerateTableDdl(DatabaseConnection databaseConnection, string schema, string tableName);
        public abstract string GenerateTableFullDdl(DatabaseConnection databaseConnection, string schema, string tableName);
    }
}
