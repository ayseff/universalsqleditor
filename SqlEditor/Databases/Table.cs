using System.Collections.Generic;

namespace SqlEditor.Databases
{
    public class Table : DatabaseObjectWithColumns
    {
        public Table(string name, DatabaseObject parent)
            : base(name, parent)
        {
            Indexes = new List<Index>();
            Constraints = new List<Constraint>();
            Partitions = new List<Partition>();
        }

        public Table()
        {
            Indexes = new List<Index>();
            Constraints = new List<Constraint>();
            Partitions = new List<Partition>();
        }

        public IList<Index> Indexes { get; private set; }

        public IList<Constraint> Constraints { get; private set; }

        public IList<Partition> Partitions { get; private set; }
    }
}