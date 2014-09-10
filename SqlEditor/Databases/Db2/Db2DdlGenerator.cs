using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using IBM.Data.DB2;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using Utilities.Process;

namespace SqlEditor.Databases.Db2
{
    public class Db2DdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string schema,
            [NotNull] string tableName)
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
            var validLines = lines.TakeWhile(line => !(line.Trim().ToCharArray().Distinct().Count() == 1 && line.Trim().ToCharArray().Distinct().FirstOrDefault() != '-')).ToList();


            return string.Join(Environment.NewLine, validLines);
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string schema, [NotNull] string tableName)
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
            return ddl;
        }

        private string RunDb2Look(string database, string user, string password, string arguments)
        {
            // Get DB2Look path
            var db2Home = ConfigurationManager.AppSettings["DB2Home"];
            if (string.IsNullOrEmpty(db2Home))
            {
                throw new Exception("DB2Home setting not set in the configuration. Please set the value for it in app.config file");
            }
            else if (!Directory.Exists(db2Home))
            {
                throw new Exception("DB2Home directory '" + db2Home + "' does not exist. Please set the correct value for it in app.config file");
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
            scriptFile = Path.Combine(Path.GetDirectoryName(scriptFile),
                Path.GetFileNameWithoutExtension(scriptFile) + ".bat");
            var scriptContents = "@echo off" + Environment.NewLine;
            scriptContents += @"@set PATH=%~d0%~p0..\db2tss\bin;%PATH%" + Environment.NewLine;
            scriptContents += @"@db2clpsetcp" + Environment.NewLine;
            scriptContents += "\"" + db2Look + "\" " + db2LookArguments;
            File.WriteAllText(scriptFile, scriptContents);
            var executor = new BackgroundProcessExecutor();
            var commandOutput = executor.RunBackgroundProcess(scriptFile, null);
            executor.VerifyExitCode(commandOutput, 0);
            return commandOutput.StandardOutput;
        }
    }
}