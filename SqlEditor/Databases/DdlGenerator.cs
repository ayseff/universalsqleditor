using System.Linq;
using System.Threading.Tasks;
using SqlEditor.DatabaseExplorer;
using Utilities.Text;

namespace SqlEditor.Databases
{
    public abstract class DdlGenerator
    {
        public abstract string GenerateCreateTableDdl(DatabaseConnection databaseConnection, string database, string schema, string tableName);

        public Task<string> GenerateCreateTableDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateCreateTableDdl(databaseConnection, database, schema, tableName));

        }
        public abstract string GenerateCreateTableFullDdl(DatabaseConnection databaseConnection, string database, string schema, string tableName);
        public Task<string> GenerateCreateTableFullDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateCreateTableFullDdl(databaseConnection, database, schema, tableName));

        }

        public virtual string GenerateDropTableDdl(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return "DROP TABLE " + GetFullyQualifiedName(database, schema, tableName) +
                   databaseConnection.DatabaseServer.SqlTerminators.First();

        }
        public Task<string> GenerateDropTableDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateDropTableDdl(databaseConnection, database, schema, tableName));

        }

        public abstract string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName);
        public Task<string> GenerateCreateViewDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateCreateViewDdl(databaseConnection, database, schema, tableName));
            //return Task.Run(() => GenerateCreateTableDdl(databaseConnection, database, schema, tableName));

        }
        public abstract string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName);
        public Task<string> GenerateCreateViewFullDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateCreateViewFullDdl(databaseConnection, database, schema, tableName));

        }

        public virtual string GenerateDropViewDdl(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return "DROP VIEW " + GetFullyQualifiedName(database, schema, tableName) +
                   databaseConnection.DatabaseServer.SqlTerminators.First();

        }
        public Task<string> GenerateDropViewDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateDropViewDdl(databaseConnection, database, schema, tableName));

        }

        public abstract string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, string indexSchema, string indexName, object indexId);
        public Task<string> GenerateCreateIndexDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName, object indexId)
        {
            return Task.Run(() => GenerateCreateIndexDdl(databaseConnection, database, schema, tableName, indexId));

        }

        public virtual string GenerateDropIndexDdl(DatabaseConnection databaseConnection, string database, string schema,
            string indexName)
        {
            return "DROP INDEX " + GetFullyQualifiedName(database, schema, indexName) +
                   databaseConnection.DatabaseServer.SqlTerminators.First();

        }
        public Task<string> GenerateDropIndexDdlAsync(DatabaseConnection databaseConnection, string database, string schema,
            string tableName)
        {
            return Task.Run(() => GenerateDropIndexDdl(databaseConnection, database, schema, tableName));

        }

        public abstract string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema, string packageName);
        public Task<string> GenerateCreatePackageDdlAsync(DatabaseConnection databaseConnection, string database, string schema, string packageName)
        {
            return Task.Run(() => GenerateCreatePackageDdl(databaseConnection, database, schema, packageName));

        }

        public static string GetFullyQualifiedName(string database, string schemaName, string objectName)
        {
            var name = string.Empty;
            var separator = string.Empty;
            if (!database.IsNullEmptyOrWhitespace())
            {
                name += database;
                separator = ".";
            }
            if (!schemaName.IsNullEmptyOrWhitespace())
            {
                name += separator + schemaName;
                separator = ".";
            }
            if (!objectName.IsNullEmptyOrWhitespace())
            {
                name += separator + objectName;
            }
            return name;
        }
    }
}
