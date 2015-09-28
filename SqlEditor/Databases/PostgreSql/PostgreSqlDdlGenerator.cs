using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using JetBrains.Annotations;
using log4net;
using SqlEditor.DatabaseExplorer;
using Utilities.Process;
using Utilities.Text;

namespace SqlEditor.Databases.PostgreSql
{
    public class PostgreSqlDdlGenerator : DdlGenerator
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Regex _trailingSpaceRegex = new Regex("\"\\s*(?<text>[^\\s]+)\\s+\"\\.", RegexOptions.Compiled);

        public override string GenerateCreateTableDdl([NotNull] DatabaseConnection databaseConnection, string database,
            [NotNull] string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var sql = GetPgDumpOutput(databaseConnection);

            // Find create table
            var tableDdl = ExtractCreateTableStatement(schema, tableName, sql);

            // Find any alter tables
            tableDdl += ExtractAlterTableStatements(schema, tableName, sql);

            return tableDdl;
        }

        public override string GenerateCreateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database,
            [NotNull] string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var sql = GetPgDumpOutput(databaseConnection);

            // Find create table
            var tableDdl = ExtractCreateTableStatement(schema, tableName, sql);

            // Find any alter tables
            tableDdl += ExtractAlterTableStatements(schema, tableName, sql);

            // Find any indexes
            tableDdl += ExtractCreateIndexStatementsForTable(schema, tableName, sql);

            return tableDdl;
        }

        private string GetPgDumpOutput(DatabaseConnection databaseConnection)
        {
            var connectionStringBuilder = (Npgsql.NpgsqlConnectionStringBuilder)
                databaseConnection.DatabaseServer.GetConnectionStringBuilder(databaseConnection.ConnectionString);
            var ddl = RunPgDump(connectionStringBuilder.Host, connectionStringBuilder.Database, connectionStringBuilder.Username,
                connectionStringBuilder.Password, connectionStringBuilder.Port);
            var sql = ddl.Replace("\r", string.Empty);
            if (sql.Length == 0)
            {
                throw new Exception("pg_dump.exe did not return any output");
            }
            return sql;
        }

        private static string ExtractCreateIndexStatementsForTable(string schema, string tableName, string sql)
        {
            //CREATE [ UNIQUE ] INDEX [ CONCURRENTLY ] [ name ] ON table [ USING method ]
            var createIndexregex =
                new Regex(string.Format(@"^\s*CREATE\s+(UNIQUE\s+)?INDEX\s+(CONCURRENTLY\s+)?\w+?\s+ON\s+({0}\.)?{1}\b\s*.*?;\s*$", schema, tableName),
                    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            var ddl = string.Empty;
            var startIndex = 0;
            var match = createIndexregex.Match(sql, startIndex);
            while (match.Success)
            {
                ddl += match.Value.Trim() + Environment.NewLine + Environment.NewLine;
                startIndex = match.Index + match.Length + 1;
                if (startIndex < sql.Length)
                {
                    match = createIndexregex.Match(sql, startIndex);
                }
                else
                {
                    break;
                }
            }
            return ddl;
        }

        private static string ExtractAlterTableStatements(string schema, string tableName, string sql)
        {
            var ddl = string.Empty;
            var alterTableRegex =
                new Regex(string.Format(@"^\s*ALTER\s+TABLE\s+(ONLY\s+)?({0}\.)?{1}\s+.*?;\s*$", schema, tableName),
                    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            var startIndex = 0;
            var match = alterTableRegex.Match(sql, startIndex);
            while (match.Success)
            {
                ddl += match.Value.Trim() + Environment.NewLine + Environment.NewLine;
                startIndex = match.Index + match.Length + 1;
                if (startIndex < sql.Length)
                {
                    match = alterTableRegex.Match(sql, startIndex);
                }
                else
                {
                    break;
                }
            }
            return ddl;
        }

        private static string ExtractCreateTableStatement(string schema, string tableName, string sql)
        {
            var createTableRegex = new Regex(
                string.Format(@"^\s*CREATE\s+((GLOBAL|LOCAL)\s+)?((TEMPORARY|TEMP)\s+)?((UNLOGGED)?\s+)?TABLE\s+((IF NOT EXISTS)\s+)?({0}\.)?{1}\b\s*\(.*?;\s*$", schema, tableName),
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            var match = createTableRegex.Match(sql);
            if (match.Success)
            {
                return match.Value.Trim() + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                _log.ErrorFormat("Could not find CREATE TABLE statement in SQL dump: {0}", sql);
                throw new Exception("Could not find CREATE TABLE statement from the pg_dump output");
            }
        }

        public override string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database,
            [NotNull] string schema, string viewName)
        {
            var sql = GetPgDumpOutput(databaseConnection);
            var createTableRegex = new Regex(
                string.Format(@"^\s*CREATE\s+(OR\s+REPLACE\s+)?((TEMPORARY|TEMP)\s+)?VIEW\s+({0}\.)?{1}\b.*?;\s*$", schema, viewName),
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            var match = createTableRegex.Match(sql);
            if (match.Success)
            {
                return match.Value.Trim() + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                _log.ErrorFormat("Could not find CREATE VIEW statement in SQL dump: {0}", sql);
                throw new Exception("Could not find CREATE VIEW statement from the pg_dump output");
            }
        }

        public override string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database,
            [NotNull] string schema, string viewName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (viewName == null) throw new ArgumentNullException("viewName");

            return GenerateCreateViewDdl(databaseConnection, database, schema, viewName);
        }

        public override string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, [NotNull] string indexSchema, string indexName, object indexId)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (indexSchema == null) throw new ArgumentNullException("indexSchema");
            if (indexName == null) throw new ArgumentNullException("indexName");

            var sql = GetPgDumpOutput(databaseConnection);
            var  createIndexRegex =
                new Regex(string.Format(@"^\s*CREATE\s+(UNIQUE\s+)?INDEX\s+(CONCURRENTLY\s+)?({0}\.)?{1}\s+ON\s+.*?;\s*$", indexSchema, indexName),
                    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            var match = createIndexRegex.Match(sql);
            if (match.Success)
            {
                return match.Value.Trim() + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                _log.ErrorFormat("Could not find CREATE INDEX statement in SQL dump: {0}", sql);
                throw new Exception("Could not find CREATE INDEX statement from the pg_dump output");
            }
        }

        public override string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema,
            string packageName)
        {
            throw new NotImplementedException();
        }


        private string RunPgDump([NotNull] string host, [NotNull] string database, [NotNull] string user,
            [NotNull] string password, int port)
        {
            if (host.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("host");
            if (database.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("database");
            if (user.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("user");
            if (password.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("password");
            // Get pg_dump path
            var postgreSqlHome = ConfigurationManager.AppSettings["PostgreSQLHome"];
            if (string.IsNullOrEmpty(postgreSqlHome))
            {
                throw new Exception("PostgreSQLHome setting not set in the configuration. Please set the value for it in " + Application.ProductName + ".exe.config file");
            }
            else if (!Directory.Exists(postgreSqlHome))
            {
                throw new Exception("PostgreSQLHome directory '" + postgreSqlHome + "' does not exist. Please set the value for it in " + Application.ProductName + ".exe.config file");
            }
            var pgDump = Path.Combine(postgreSqlHome, "pg_dump.exe");
            if (!File.Exists(pgDump))
            {
                throw new Exception("pg_dump.exe executable '" + pgDump + "' does not exist. Please make sure you have it properly installed");
            }

            // pg_dump.exe --username=mmedic --schema-only --host=localhost Test
            var pgDumpArguments =
                string.Format(
                    "--host={0} --port={1} --username={2} --schema-only --no-password {3} ", host, port, user, database);

            // Build script
            var tempFile = Path.GetTempFileName();
            var scriptFile = Path.Combine(Path.GetDirectoryName(tempFile) ?? string.Empty,
                    Path.GetFileNameWithoutExtension(tempFile) + ".bat");
            try
            {
                File.Move(tempFile, scriptFile);
                var scriptContents = "@echo off" + Environment.NewLine;
                scriptContents += @"@set PGPASSWORD=" + password + Environment.NewLine;
                scriptContents += "\"" + pgDump + "\" " + pgDumpArguments;
                File.WriteAllText(scriptFile, scriptContents);
                var executor = new BackgroundProcessExecutor();
                var commandOutput = executor.RunBackgroundProcess(scriptFile, null);
                if (commandOutput.ExitCode != 0)
                {
                    throw new Exception("pg_dump.exe returned unsuccessful return code of " + commandOutput.ExitCode + ". " + commandOutput.StandardError + Environment.NewLine + commandOutput.StandardError);
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
            output = output.Replace("\"", string.Empty);
            return output;
        }
    }
}