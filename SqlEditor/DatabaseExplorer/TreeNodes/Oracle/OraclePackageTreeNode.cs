using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Oracle
{
    public sealed class OraclePackageTreeNode : PackageTreeNode
    {
        public OraclePackageTreeNode(Package package, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(package, databaseConnection, databaseInstance)
        {
        }
    }
}