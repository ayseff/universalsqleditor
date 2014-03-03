using System;
using SqlEditor.Database;

namespace SqlEditor.Databases
{
    public class Partition : DatabaseObject
    {
        public Partition()
        {
        }

        public Partition(string name, DatabaseObject parent) : base(name, parent)
        {
        }

        public Partition(string name, string displayName, DatabaseObject parent) : base(name, displayName, parent)
        {
        }

        public override void Clear()
        {
        }
    }
}