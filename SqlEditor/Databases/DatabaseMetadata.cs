using System;
using System.Collections.Concurrent;
using SqlEditor.Database;

namespace SqlEditor.Databases
{
    public class DatabaseMetadata
    {
        #region Fields

        private readonly ConcurrentDictionary<string, Schema> _allSchemas = new ConcurrentDictionary<string, Schema>();
        private ConcurrentDictionary<string, Column> _allColumns = new ConcurrentDictionary<string, Column>();

        private ConcurrentDictionary<string, Index> _allIndexes = new ConcurrentDictionary<string, Index>();

        private ConcurrentDictionary<string, MaterializedView> _allMaterializedViews =
            new ConcurrentDictionary<string, MaterializedView>();

        private ConcurrentDictionary<string, Sequence> _allSequences = new ConcurrentDictionary<string, Sequence>();
        private ConcurrentDictionary<string, Table> _allTables = new ConcurrentDictionary<string, Table>();
        private ConcurrentDictionary<string, View> _allViews = new ConcurrentDictionary<string, View>();
        private Database _database;
        private DbInfoProvider _infoProvider;
        private Schema _schema;

        #endregion

        public DatabaseMetadata(Database database)
        {
            _database = database;
            _infoProvider = database.GetInfoProvider();
            _schema = GetOrCreateSchema(database.GetLoginUserId());
        }

        public Schema GetOrCreateSchema(string schemaName)
        {
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            schemaName = schemaName.Trim().ToUpper();
            Schema schema;
            if (_allSchemas.TryGetValue(schemaName, out schema))
            {
                return schema;
            }
            schema = new Schema(schemaName);
            return null;
        }
    }
}