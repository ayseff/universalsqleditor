using System;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteDdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            using (var connection = databaseConnection.CreateNewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT sql FROM sqlite_master WHERE lower(type) = 'table' AND upper(name) = upper(@1)";
                    var param = command.CreateParameter();
                    param.ParameterName = "@1";
                    param.Value = tableName;
                    command.Parameters.Add(param);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return dr.GetString(0);
                        }
                        throw new Exception("SQLite sqlite_master table does not have an entry for the table " + tableName);
                    }
                }
            }
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            throw new NotImplementedException();
        }
    }
}