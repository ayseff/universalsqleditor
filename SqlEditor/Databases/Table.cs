using System.Collections.Generic;
using SqlEditor.Database;

namespace SqlEditor.Databases
{
    public class Table : DatabaseObjectWithColumns
    {
        public Table(string name, DatabaseObject parent)
            : base(name, parent)
        {
            Indexes = new List<Index>();
            Partitions = new List<Partition>();
        }

        public Table()
        {
            Indexes = new List<Index>();
            Partitions = new List<Partition>();
        }

        public IList<Index> Indexes { get; private set; }

        public IList<Partition> Partitions { get; private set; }
    }
}