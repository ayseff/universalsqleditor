using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;

namespace SqlEditor.SqlHistory
{
    public class SqlHistoryImportExport
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(SqlHistoryXml));

        public static List<ExecutedSqlStatement> LoadExecutedSqlStatements(string xmlFileName)
        {
            if (xmlFileName == null) throw new ArgumentNullException("xmlFileName");
            var executedSqlStatements = new List<ExecutedSqlStatement>();
            try
            {
                _log.Debug("Loading sql statements ...");
                if (!File.Exists(xmlFileName))
                {
                    string message = string.Format(
                        "Could not load sql statements from file {0}. The file does not exist!", xmlFileName);
                    _log.Error(message);
                    throw new Exception(message);
                }

                SqlHistoryXml sqlHistoryXml;
                using (var stream = new StreamReader(xmlFileName, Encoding.Unicode))
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        sqlHistoryXml = (SqlHistoryXml)_serializer.Deserialize(reader);
                    }
                }

                foreach (var sqlStatementXml in sqlHistoryXml.Items)
                {
                    var executedSqlStatement = new ExecutedSqlStatement
                        {ConnectionName = sqlStatementXml.ConnectionName, SqlStatement = sqlStatementXml.SqlText, RunDateTime = DateTime.MinValue};
                    DateTime dateTime;
                    if (DateTime.TryParse(sqlStatementXml.RunDateTime, out dateTime))
                    {
                        executedSqlStatement.RunDateTime = dateTime;
                    }
                    executedSqlStatements.Add(executedSqlStatement);
                }
                _log.DebugFormat("Loaded {0} sql statement(s).", executedSqlStatements.Count.ToString("#,0"));
                return executedSqlStatements;
            }
            catch (Exception ex)
            {
                _log.Error("Error loading sql statements.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        public static void SaveExecutedSqlStatements(IList<ExecutedSqlStatement> executedSqlStatements, string xmlFileName)
        {
            try
            {
                _log.Debug("Saving sql statements ...");
                var sqlStatementsXml = new SqlHistoryXml { Items = new SqlStatement[executedSqlStatements.Count] };
                for (var i = 0; i < executedSqlStatements.Count; i++)
                {
                    var executedSqlStatement = executedSqlStatements[i];
                    var sqlStatement = new SqlStatement
                        {
                        ConnectionName = executedSqlStatement.ConnectionName,
                        RunDateTime = executedSqlStatement.RunDateTime.ToString("G"),
                        SqlText = executedSqlStatement.SqlStatement
                    };
                    sqlStatementsXml.Items[i] = sqlStatement;
                }

                using (var stream = new FileStream(xmlFileName, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        _serializer.Serialize(writer, sqlStatementsXml);
                    }
                }
                _log.DebugFormat("Saved {0} sql statement(s).", executedSqlStatements.Count.ToString("#,0"));
            }
            catch (Exception ex)
            {
                _log.Error("Error saving sql statements.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
