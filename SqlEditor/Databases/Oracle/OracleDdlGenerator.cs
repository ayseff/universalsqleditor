using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases.Oracle
{
    public class OracleDdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string database,
            [NotNull] string schema, 
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            // Get full DDL
            var ddl = RunDbmsMetadata(databaseConnection, "TABLE", schema, tableName);
            ddl += Environment.NewLine + databaseConnection.DatabaseServer.SqlTerminators[0] + Environment.NewLine;
            return ddl;
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, [NotNull] string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var ddl = GenerateTableDdl(databaseConnection, database, schema, tableName) + Environment.NewLine + Environment.NewLine;

            // Get indexes
            var indexDdl = RunDbmsDependentMetadata(databaseConnection, "INDEX", schema, tableName).Trim();
            var lines = indexDdl.Split(new[] {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
            var linesList = new List<string> {lines[0]};
            for (var i = 1; i < lines.Count; i++)
            {
                if (Regex.IsMatch(lines[i].Trim(), @"^CREATE\s+INDEX", RegexOptions.IgnoreCase))
                {
                    linesList.Add(databaseConnection.DatabaseServer.SqlTerminators[0] + Environment.NewLine+Environment.NewLine);
                }
                linesList.Add(lines[i]);
            }

            linesList.Add(databaseConnection.DatabaseServer.SqlTerminators[0] + Environment.NewLine + Environment.NewLine);
            ddl += string.Join(Environment.NewLine, linesList);
            return ddl;
        }

        private static string RunDbmsMetadata([NotNull] DatabaseConnection databaseConnection, [NotNull] string objectType, [NotNull] string schema,
            [NotNull] string objectName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (objectName == null) throw new ArgumentNullException("objectName");

            return RunDbmsMetadataInternal(databaseConnection, "GET_DDL", objectType, schema, objectName);
        }

        private static string RunDbmsDependentMetadata([NotNull] DatabaseConnection databaseConnection, [NotNull] string objectType, [NotNull] string schema,
            [NotNull] string objectName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (objectName == null) throw new ArgumentNullException("objectName");

            return RunDbmsMetadataInternal(databaseConnection, "GET_DEPENDENT_DDL", objectType, schema, objectName);
        }

        private static string RunDbmsMetadataInternal([NotNull] DatabaseConnection databaseConnection,
            [NotNull] string dmbsMetaDataType, [NotNull] string objectType, [NotNull] string schema,
            [NotNull] string objectName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (dmbsMetaDataType == null) throw new ArgumentNullException("dmbsMetaDataType");
            if (schema == null) throw new ArgumentNullException("schema");
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (objectName == null) throw new ArgumentNullException("objectName");

            using (var connection = databaseConnection.CreateNewConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT DBMS_METADATA.{0}('{1}', '{2}', '{3}') from DUAL", dmbsMetaDataType.Trim().ToUpper(), objectType.Trim().ToUpper(), objectName.Trim().ToUpper(), schema.Trim().ToUpper());
                var ddl = ((string)command.ExecuteScalar()).Trim();
                return ddl;
            }
        }
    }
}