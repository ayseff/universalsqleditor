using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Annotations;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TableConstraintsTreeNode : FolderContainerTreeNode
    {
        public Table Table { get; set; }

        public TableConstraintsTreeNode([NotNull] Table table, DatabaseConnection connection, DatabaseInstance databaseInstance)
            : base("Constraints", connection, databaseInstance)
        {
            if (table == null) throw new ArgumentNullException("table");

            Table = table;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                _log.DebugFormat("Loading constraints for table {0} ...", Table.FullyQualifiedName);
                Table.Constraints.Clear();
                IList<Constraint> constraints;
                using (var connection = DatabaseConnection.CreateNewConnection())
                {
                    connection.OpenIfRequired();
                    var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                    var databaseInstanceName = Table.Parent.Parent == null ? null : Table.Parent.Parent.Name;
                    constraints = infoProvider.GetConstraintsForTable(connection, Table.Parent.Name, Table.Name, databaseInstanceName);
                    foreach (var constraint in constraints)
                    {
                        constraint.Parent = Table.Parent;
                    }
                }
                Table.Constraints.AddRange(constraints);
                _log.DebugFormat("Loaded {0} constraint(s).", constraints.Count);

                var constraintNodes =
                    constraints.Select(x => new ConstraintTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
                return constraintNodes;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading constraints for table {0}.", Table.Name);
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}