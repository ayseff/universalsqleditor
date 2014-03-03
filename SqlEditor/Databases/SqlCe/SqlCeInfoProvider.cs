﻿using System;
using System.Collections.Generic;
using System.Data;
using SqlEditor.Database;
using SqlEditor.Intellisense;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeInfoProvider : DbInfoProvider
    {
        private const string DEFAULT_SCHEMA = "MAIN";
        private static readonly Schema _defaultSchema = new Schema(string.Empty, DEFAULT_SCHEMA);

        public override IList<Schema> GetSchemas(IDbConnection connection)
        {
            return new List<Schema>(new[] { _defaultSchema });
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTablesBase(connection, schemaName,
                                 "SELECT table_name FROM information_schema.tables WHERE TABLE_TYPE <> N'SYSTEM TABLE' ORDER BY table_name");
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTableColumnsBase(connection, schemaName, tableName,
                                       "SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position FROM information_schema.columns WHERE UPPER(table_name) = @1 ORDER BY ordinal_position",
                                       tableName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTableColumnsBase(connection, schemaName, tableName,
                                       "SELECT c.column_name, c.data_type, c.character_maximum_length, c.numeric_precision, c.numeric_scale, c.is_nullable, c.ordinal_position  FROM information_schema.columns c INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC 	ON UPPER(c.table_name) = UPPER(tc.table_name) INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU 	ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME AND c.column_name = ku.column_name WHERE UPPER(c.table_name) = @1  ORDER BY ordinal_position",
                                       tableName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName)
        {
            return new List<Partition>();
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName)
        {
            return new List<View>();
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName)
        {
            return new List<Column>();
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName)
        {
            return new List<MaterializedView>();
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName,
                                                                 string materializedViewName)
        {
            return new List<Column>();
        }

        public override IList<Index> GetIndexes(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT DISTINCT '" + DEFAULT_SCHEMA + "', idx.index_name, CASE WHEN idx.[unique]= 1 THEN 1 ELSE 0 END AS is_unique  FROM information_schema.indexes as idx ORDER BY idx.index_name");
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT DISTINCT '" + DEFAULT_SCHEMA + "', idx.index_name, CASE WHEN idx.[unique]= 1 THEN 1 ELSE 0 END AS is_unique  FROM information_schema.indexes as idx WHERE UPPER(table_name) = @1 ORDER BY idx.index_name", tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns(IDbConnection connection, string schemaName, string indexName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (indexName == null) throw new ArgumentNullException("indexName");
            return GetIndexColumnsBase(connection, _defaultSchema.Name, indexName,
                                       "SELECT c.column_name, c.data_type, c.character_maximum_length, c.numeric_precision, c.numeric_scale, c.is_nullable, c.ordinal_position FROM information_schema.indexes s INNER JOIN information_schema.columns c ON c.table_name = s.TABLE_NAME AND c.column_name = s.column_name WHERE UPPER(index_name) = @1",
                                       indexName.Trim().ToUpper());
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schema)
        {
            return new List<Sequence>();
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName)
        {
            return new List<Trigger>();
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName)
        {
            return new List<Synonym>();
        }

        public override IList<Synonym> GetSynonyms(IDbConnection connection, string schemaName)
        {
            return new List<Synonym>();
        }

        public override IList<StoredProcedure> GetStoredProcedures(IDbConnection connection, string schemaName)
        {
            return new List<StoredProcedure>();
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
        {
            return new List<ColumnParameter>();
        }

        public override IntelisenseData GetIntelisenseData(IDbConnection connection, string currentSchemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            
            const string tablesSql =
                "SELECT 'MAIN', table_name, column_name FROM information_schema.columns ORDER BY table_name, column_name";

            return GetIntellisenseDataHelper(connection, currentSchemaName, tablesSql, null, null);
        }
    }
}
