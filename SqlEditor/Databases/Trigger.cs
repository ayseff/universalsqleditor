using SqlEditor.Database;

namespace SqlEditor.Databases
{
    public class Trigger : DatabaseObject
    {
        public Table Table { get; set; }

        public Trigger(string name, DatabaseObject parent) : base(name, parent)
        {
        }

        public Trigger(string name, string displayName, DatabaseObject parent) : base(name, displayName, parent)
        {
        }

        public Trigger()
        {
        }

        public override void Clear()
        {
        }
    }
}