using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases
{
    public abstract class DdlGenerator
    {
        public abstract string GenerateTableDdl(DatabaseConnection databaseConnection, string database, string schema, string tableName);
        public abstract string GenerateTableFullDdl(DatabaseConnection databaseConnection, string database, string schema, string tableName);
        public abstract string GenerateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName);

        public abstract string GenerateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName);
    }
}
