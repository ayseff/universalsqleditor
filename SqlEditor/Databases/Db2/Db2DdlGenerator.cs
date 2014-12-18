using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using IBM.Data.DB2;
using JetBrains.Annotations;
using log4net;
using SqlEditor.DatabaseExplorer;
using Utilities.Process;

namespace SqlEditor.Databases.Db2
{
    public class Db2DdlGenerator : DdlGenerator
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Regex _trailingSpaceRegex = new Regex("\"\\s*(?<text>[^\\s]+)\\s+\"\\.", RegexOptions.Compiled);

        public override string GenerateCreateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            // Get full DDL
            var ddl = GenerateCreateTableFullDdl(databaseConnection, null, schema, tableName);

            // Find start of create table
            var lines = ddl.Split(new []{ "\n" }, StringSplitOptions.None ).ToList();
            while (lines.Count > 0)
            {
                if (lines[0].Trim().StartsWith("CREATE TABLE", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                lines.RemoveAt(0);
            }

            // Find end of create table
            var validLines = new List<string>();
            foreach (var line in lines)
            {
                var chars = line.Trim().ToCharArray().Distinct().ToList();
                if (chars.Count == 1 && chars[0] == '-')
                {
                    break;
                }
                validLines.Add(line);
            }


            var sql = string.Join("\n", validLines);
            sql = sql.Replace("\r", string.Empty);
            return sql;
        }

        public override string GenerateCreateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var connectionStringBuilder = (DB2ConnectionStringBuilder)
                databaseConnection.DatabaseServer.GetConnectionStringBuilder(databaseConnection.ConnectionString);
            var arguments = string.Format("-t {0} ", tableName);
            if (!string.IsNullOrEmpty(schema))
            {
                arguments += string.Format("-z {0} ", schema);
            }
            var ddl = RunDb2Look(connectionStringBuilder.Database, connectionStringBuilder.UserID,
                connectionStringBuilder.Password, arguments);
            var sql = ddl.Replace("\r", string.Empty);
            return sql;
        }

        public override string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (viewName == null) throw new ArgumentNullException("viewName");

            // Get full DDL
            var ddl = GenerateCreateViewFullDdl(databaseConnection, null, schema, viewName);

            // Find start of create table
            var lines = ddl.Split(new[] { "\n" }, StringSplitOptions.None).ToList();
            while (lines.Count > 0)
            {
                if (lines[0].Trim().StartsWith("CREATE VIEW", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                lines.RemoveAt(0);
            }

            // Find end of create table
            var validLines = new List<string>();
            foreach (var line in lines)
            {
                if (line.Trim() == string.Empty)
                {
                    break;
                }
                validLines.Add(line);
            }


            var sql = string.Join("\n", validLines);
            sql = sql.Replace("\r", string.Empty);
            return sql;
        }

        public override string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (viewName == null) throw new ArgumentNullException("viewName");

            var connectionStringBuilder = (DB2ConnectionStringBuilder)
                databaseConnection.DatabaseServer.GetConnectionStringBuilder(databaseConnection.ConnectionString);
            var arguments = string.Format("-v {0} ", viewName);
            if (!string.IsNullOrEmpty(schema))
            {
                arguments += string.Format("-z {0} ", schema);
            }
            var ddl = RunDb2Look(connectionStringBuilder.Database, connectionStringBuilder.UserID,
                connectionStringBuilder.Password, arguments);
            var sql = ddl.Replace("\r", string.Empty);
            return sql;
        }

        public override string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, [NotNull] string indexSchema, string indexName, object indexId)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (indexSchema == null) throw new ArgumentNullException("indexSchema");
            if (indexName == null) throw new ArgumentNullException("indexName");

            // Find table name for this index
            string tableSchema, tableName;
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "select TABSCHEMA, TABNAME from syscat.INDEXES where UPPER(INDSCHEMA) = @1 and UPPER(INDNAME) = @2 fetch first row only";
                    var param = command.CreateParameter();
                    param.ParameterName = "@1";
                    param.Value = indexSchema.Trim().ToUpper();
                    command.Parameters.Add(param);
                    param = command.CreateParameter();
                    param.ParameterName = "@2";
                    param.Value = indexName.Trim().ToUpper();
                    command.Parameters.Add(param);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tableSchema = dr.GetString(0).Trim().ToUpper();
                            tableName = dr.GetString(1).Trim().ToUpper();
                        }
                        else
                        {
                            throw new Exception("Index " + indexSchema + "." + indexName + " does not exist in the database");
                        }
                    }
                }
            }

            // Get full DDL
            var tableDdl = GenerateCreateTableFullDdl(databaseConnection, null, tableSchema, tableName);

            // Find start of create index
            var indexDdl = new StringBuilder();
            var startCapture = false;
            var regex = new Regex("CREATE\\s+INDEX\\s+(\")?(?<schema>\\w+)(\")?\\.(\")?(?<index>\\w+)(\")?",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var lines = tableDdl.Split(new[] { "\n" }, StringSplitOptions.None).ToList();
            foreach (var line in lines)
            {
                if (!startCapture)
                {
                    var match = regex.Match(line);
                    if (match.Success
                        &&
                        string.Equals(match.Groups["schema"].Value, indexSchema, StringComparison.InvariantCultureIgnoreCase)
                        &&
                        string.Equals(match.Groups["index"].Value, indexName,
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        startCapture = true;
                        indexDdl.AppendLine(line);
                    }
                }
                else if (line.Trim() == string.Empty)
                {
                    break;
                }
                else
                {
                    indexDdl.AppendLine(line);
                }
            }
            return indexDdl.ToString();
        }

        public override string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema,
            string packageName)
        {
            throw new NotImplementedException();
        }


        private string RunDb2Look(string database, string user, string password, string arguments)
        {
            // Get DB2Look path
            var db2Home = ConfigurationManager.AppSettings["DB2Home"];
            if (string.IsNullOrEmpty(db2Home))
            {
                throw new Exception("DB2Home setting not set in the configuration. Please set the value for it in " + Application.ProductName + ".exe.config file");
            }
            else if (!Directory.Exists(db2Home))
            {
                throw new Exception("DB2Home directory '" + db2Home + "' does not exist. Please set the value for it in " + Application.ProductName + ".exe.config file");
            }
            var db2Look = Path.Combine(db2Home, "db2look.exe");
            if (!File.Exists(db2Look))
            {
                throw new Exception("db2look executable '" + db2Look + "' does not exist. Please make sure you have a DB2 client properly installed");
            }

            var db2LookArguments =
                string.Format(
                    "-d {0} -i {1} -w {2} -a -e -x ", database, user, password) + arguments;

            // Build script
            var scriptFile = Path.GetTempFileName();
            try
            {
                scriptFile = Path.Combine(Path.GetDirectoryName(scriptFile) ?? string.Empty,
                    Path.GetFileNameWithoutExtension(scriptFile) + ".bat");
                var scriptContents = "@echo off" + Environment.NewLine;
                scriptContents += @"@set PATH=%~d0%~p0..\db2tss\bin;%PATH%" + Environment.NewLine;
                scriptContents += @"@cd " + db2Home + @"\..\bnd" + Environment.NewLine;
                scriptContents += @"@db2clpsetcp" + Environment.NewLine;
                scriptContents += "\"" + db2Look + "\" " + db2LookArguments;
                File.WriteAllText(scriptFile, scriptContents);
                var executor = new BackgroundProcessExecutor();
                var commandOutput = executor.RunBackgroundProcess(scriptFile, null);
                if (commandOutput.ExitCode != 0)
                {
                    throw new Exception("db2look.exe returned unsuccessful return code of " + commandOutput.ExitCode + ". " + commandOutput.StandardError + Environment.NewLine + commandOutput.StandardError);
                }

                // Clean up standard output
                var output = commandOutput.StandardOutput;
                output = CleanText(output, _trailingSpaceRegex);
                return output;
            }
            finally 
            {
                try
                {
                    File.Delete(scriptFile);
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error deleting file {0}. {1}", scriptFile, ex.Message);
                    _log.Error(ex.Message, ex);
                }
            }
        }

        private static string CleanText(string output, Regex regex)
        {
            var match = regex.Match(output);
            while (match.Success)
            {
                var part1 = output.Substring(0, match.Index);
                var part2 = match.Groups["text"].Value;
                var part3 = output.Substring(match.Index + match.Length);
                output = part1 + "\"" + part2 + "\"." + part3;
                match = regex.Match(output);
            }
            return output;
        }
    }
}