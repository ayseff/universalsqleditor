using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class PublicSynonymsTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; private set; }

        public PublicSynonymsTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base("Public Synonyms", databaseConnection, databaseInstance)
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
                synonyms = infoProvider.GetPublicSynonyms(connection, Schema.Name, databaseInstanceName);
                foreach (var synonym in synonyms)
                {
                    synonym.Parent = Schema;
                }
            }
            //Schema.Synonyms.AddRange(synonyms);
            _log.DebugFormat("Loaded {0} public synonym(s).", synonyms.Count);

            var nodes = synonyms.Select(x => new SynonymTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}