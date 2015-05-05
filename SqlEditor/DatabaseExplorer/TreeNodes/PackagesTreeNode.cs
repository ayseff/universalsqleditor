using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class PackagesTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; private set; }

        public PackagesTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance, string nodeDisplayText = "Packages")
            : base(nodeDisplayText, databaseConnection, databaseInstance)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading packages ...");
            IList<Package> packages;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                packages = infoProvider.GetPackages(connection, Schema.Name);
                foreach (var package in packages)
                {
                    package.Parent = Schema;
                }
            }
            _log.DebugFormat("Loaded {0} package(s).", packages.Count);

            var nodes = packages.Select(package => new PackageTreeNode(package, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}