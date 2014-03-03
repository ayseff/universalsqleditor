using System;
using Oracle.ManagedDataAccess.Client;
using log4net;

namespace SqlEditor
{
    public class DdlGenerator
    {
        private static ILog _log = LogManager.GetLogger(typeof(DdlGenerator));

        private static string GenerateDdl(string ddlType, string objectType, string schemaName, string tableName, OracleConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format("select DBMS_METADATA.{0}('{1}', '{2}', '{3}') from DUAL", ddlType, objectType, tableName.Trim().ToUpper(), schemaName.Trim().ToUpper());
            string ddl = ((string)command.ExecuteScalar()).Trim();            
            return ddl;
        }

        public static string GenerateTableDdl(string schemaName, string tableName, OracleConnection connection)
        {
            string ddl = GenerateDdl("GET_DDL", "TABLE", schemaName, tableName, connection);
            ddl += Environment.NewLine + Environment.NewLine;
            ddl += GenerateTableIndexDdl(schemaName, tableName, connection);
            return ddl;
        }

        public static string GenerateIndexDdl(string schemaName, string indexName, OracleConnection connection)
        {
            return GenerateDdl("GET_DDL", "INDEX", schemaName, indexName, connection);
        }

        public static string GenerateTableIndexDdl(string schemaName, string tableName, OracleConnection connection)
        {
            return GenerateDdl("GET_DEPENDENT_DDL", "INDEX", schemaName, tableName, connection);
        }
    }
}
