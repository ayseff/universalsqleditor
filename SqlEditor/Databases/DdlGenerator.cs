using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases
{
    public abstract class DdlGenerator
    {
        public abstract string GenerateTableDdl(DatabaseConnection databaseConnection, string database, string schema, string tableName);
        public abstract string GenerateTableFullDdl(DatabaseConnection databaseConnection, string database, string schema, string tableName);
    }
}
