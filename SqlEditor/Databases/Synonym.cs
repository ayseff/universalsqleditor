using SqlEditor.Database;

namespace SqlEditor.Databases
{
    public class Synonym : DatabaseObject
    {
        public string TargetObjectName { get; set; }

        public Synonym(string name, string displayName, DatabaseObject parent) : base(name, displayName, parent)
        {
        }

        public Synonym()
        {
        }

        public Synonym(string name, DatabaseObject parent) : base(name, parent)
        {
        }

        public override void Clear()
        {
        }
    }
}