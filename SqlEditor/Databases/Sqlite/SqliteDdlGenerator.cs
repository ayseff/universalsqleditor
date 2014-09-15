using System;
using System.Linq;
using System.Text;
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
                            return dr.GetString(0) + Environment.NewLine +
                                   databaseConnection.DatabaseServer.SqlTerminators.FirstOrDefault();
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

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(GenerateTableDdl(databaseConnection, database, schema, tableName));
            stringBuilder.AppendLine(Environment.NewLine);

            var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var indexes = infoProvider.GetIndexesForTable(connection, string.Empty, tableName);
                foreach (var index in indexes)
                {
                    var columns = infoProvider.GetIndexColumns(connection, string.Empty, index.Name);
                    stringBuilder.AppendFormat("CREATE {0}INDEX {1} ON {2}({3})",
                        index.IsUnique ? "UNIQUE " : string.Empty, index.Name, tableName,
                        string.Join(", ", columns.Select(x => x.Name).ToList()));
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine(databaseConnection.DatabaseServer.SqlTerminators.FirstOrDefault());
                    stringBuilder.AppendLine(Environment.NewLine);
                }
            }
            return stringBuilder.ToString();
        }
    }
}