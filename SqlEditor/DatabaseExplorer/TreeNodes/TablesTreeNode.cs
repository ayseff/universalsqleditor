using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TablesTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; protected set; }

        public TablesTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection)
            : base("Tables", databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading tables ...");
            //Schema.Tables.Clear();
            IList<Table> tables;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                tables = infoProvider.GetTables(connection, Schema.Name);
            }
            //Schema.Tables.AddRange(tables);
            _log.DebugFormat("Loaded {0} table(s).", tables.Count);

            var nodes = tables.Select(table => new TableTreeNode(table, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}