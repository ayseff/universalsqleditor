using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class PackageTreeNode : TreeNodeBase
    {
        public Package Package { get; set; }

        public PackageTreeNode(Package package, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (package == null) throw new ArgumentNullException("package");

            this.Package = package;
            Text = Package.DisplayName;
            this.Override.NodeAppearance.Image =
                DatabaseExplorerImageList.Instance.ImageList.Images["package.png"];
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                _log.DebugFormat("Loading package procedures for package {0} ...", Package.FullyQualifiedName);
                Package.Procedures.Clear();
                IList<PackageProcedure> packageProcedures;
                using (var connection = DatabaseConnection.CreateNewConnection())
                {
                    connection.OpenIfRequired();
                    var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                    packageProcedures = infoProvider.GetPackageProcedures(connection, Package.Parent.Name, Package.Name);
                }
                Package.Procedures.AddRange(packageProcedures);
                _log.DebugFormat("Loaded {0} package procedure(s).", packageProcedures.Count);

                var nodes = Package.Procedures.Select(x => new PackageProcedureTreeNode(x, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
                var treeNodeBases = nodes;
                return treeNodeBases;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading package procedures for package {0}.", Package.FullyQualifiedName);
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}