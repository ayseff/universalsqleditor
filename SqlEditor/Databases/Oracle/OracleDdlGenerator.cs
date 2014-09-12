using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using IBM.Data.DB2;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using Utilities.Process;

namespace SqlEditor.Databases.Oracle
{
    public class OracleDdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string schema, string database,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            // Get full DDL
            var ddl = GenerateTableFullDdl(databaseConnection, null, schema, tableName);

            // Find start of create table
            var lines = ddl.Split(new []{ "\r\n" }, StringSplitOptions.None ).ToList();
            while (lines.Count > 0)
            {
                if (lines[0].Trim().StartsWith("CREATE TABLE", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                lines.RemoveAt(0);
            }

            // Find end of create table
            var validLines = new List<string>(); // lines.TakeWhile(line => !(line.Trim().ToCharArray().Distinct().Count() == 1 && line.Trim().ToCharArray().Distinct().FirstOrDefault() != '-')).ToList();
            foreach (var line in lines)
            {
                var chars = line.Trim().ToCharArray().Distinct().ToList();
                if (chars.Count == 1 && chars[0] == '-')
                {
                    break;
                }
                validLines.Add(line);
            }


            return string.Join(Environment.NewLine, validLines);
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, [NotNull] string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var ddl = RunDbmsMetadata(databaseConnection, "TABLE", schema, tableName);
            return ddl;
        }

        private static string RunDbmsMetadata([NotNull] DatabaseConnection databaseConnection, [NotNull] string objectType, [NotNull] string schema,
            [NotNull] string objectName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (objectName == null) throw new ArgumentNullException("objectName");

            using (var connection = databaseConnection.CreateNewConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT DBMS_METADATA.GET_DDL('{0}', '{1}', '{2}') from DUAL", objectType, objectName.Trim().ToUpper(), schema.Trim().ToUpper());
                var ddl = ((string)command.ExecuteScalar()).Trim();
                return ddl;
            }
        }
    }
}