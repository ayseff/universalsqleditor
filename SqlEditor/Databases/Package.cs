using System.Collections.Generic;

namespace SqlEditor.Databases
{
    public class Package : DatabaseObject
    {
        public List<PackageProcedure> Procedures { get; set; }

        public string Definition { get; set; }

        public Package()
        {
            Procedures = new List<PackageProcedure>();
        }

        public Package(string name, DatabaseObject parent)
            : base(name, parent)
        {
            Procedures = new List<PackageProcedure>();
        }

        public override void Clear()
        {
            Procedures.Clear();
        }
    }
}