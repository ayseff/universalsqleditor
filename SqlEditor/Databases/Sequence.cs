namespace SqlEditor.Databases
{
    public class Sequence : DatabaseObject
    {
        public decimal MinimumValue { get; set; }
        public decimal MaximumValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Increment { get; set; }

        public Sequence(string name, string displayName, DatabaseObject parent) : base(name, displayName, parent)
        {
        }

        public Sequence()
        {
        }

        public Sequence(string name, DatabaseObject parent) : base(name, parent)
        {
        }

        public override void Clear()
        {
        }
    }
}