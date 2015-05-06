using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class ConstraintsTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; private set; }

        public ConstraintsTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base("Constraints", databaseConnection, databaseInstance)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading constraints ...");
            IList<Constraint> constraints;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                var databaseInstanceName = DatabaseInstance == null ? null : DatabaseInstance.Name;
                constraints = infoProvider.GetConstraints(connection, Schema.Name, databaseInstanceName);
                foreach (var sequence in constraints)
                {
                    sequence.Parent = Schema;
                }
            }
            _log.DebugFormat("Loaded {0} constraint(s).", constraints.Count);

            var nodes = constraints.Select(x => new ConstraintTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}