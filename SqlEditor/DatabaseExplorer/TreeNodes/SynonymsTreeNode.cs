using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SynonymsTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; private set; }

        public SynonymsTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base("Synonyms", databaseConnection, databaseInstance)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading synonyms ...");
            //Schema.Tables.Clear();
            IList<Synonym> synonyms;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                var databaseInstanceName = DatabaseInstance == null ? null : DatabaseInstance.Name;
                synonyms = infoProvider.GetSynonyms(connection, Schema.Name, databaseInstanceName);
                foreach (var synonym in synonyms)
                {
                    synonym.Parent = Schema;
                }
            }
            //Schema.Synonyms.AddRange(synonyms);
            _log.DebugFormat("Loaded {0} synonym(s).", synonyms.Count);

            var nodes = synonyms.Select(x => new SynonymTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}