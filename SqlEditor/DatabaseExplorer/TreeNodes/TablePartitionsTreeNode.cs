﻿using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Annotations;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TablePartitionsTreeNode : FolderContainerTreeNode
    {
        public Table Table { get; set; }

        public TablePartitionsTreeNode([NotNull] Table table, DatabaseConnection connection)
            : base("Partitions", connection)
        {
            if (table == null) throw new ArgumentNullException("table");
            
            Table = table;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                _log.DebugFormat("Loading partitions for table {0} ...", Table.FullyQualifiedName);
                Table.Partitions.Clear();
                IList<Partition> partitions;
                using (var connection = DatabaseConnection.CreateNewConnection())
                {
                    connection.OpenIfRequired();
                    var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                    partitions = infoProvider.GetTablePartitions(connection, Table.Parent.Name, Table.Name);
                }
                Table.Partitions.AddRange(partitions);
                _log.DebugFormat("Loaded {0} partition(s).", partitions.Count);

                var partitionNodes =
                    partitions.Select(x => new PartitionTreeNode(x, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
                return partitionNodes;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading partitions for table {0}.", Table.Name);
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}