using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SynonymsTreeNode : FolderContainerTreeNode
    {
        public Schema Schema { get; protected set; }

        public SynonymsTreeNode(Schema schema, DatabaseConnection databaseConnection)
            : base("Synonyms", databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading synonyms ...");
            Schema.Tables.Clear();
            IList<Synonym> synonyms;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                synonyms = infoProvider.GetSynonyms(connection, Schema.Name);
            }
            Schema.Synonyms.AddRange(synonyms);
            _log.DebugFormat("Loaded {0} synonym(s).", synonyms.Count);

            var nodes = synonyms.Select(x => new SynonymTreeNode(x, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}