using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SequencesTreeNode : FolderContainerTreeNode
    {
        public Schema Schema { get; protected set; }

        public SequencesTreeNode(Schema schema, DatabaseConnection databaseConnection)
            : base("Sequences", databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading sequences ...");
            Schema.Tables.Clear();
            IList<Sequence> sequences;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                sequences = infoProvider.GetSequences(connection, Schema.Name);
            }
            Schema.Sequences.AddRange(sequences);
            _log.DebugFormat("Loaded {0} sequence(s).", sequences.Count);

            var nodes = sequences.Select(x => new SequenceTreeNode(x, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}