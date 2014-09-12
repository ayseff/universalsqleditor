using System.ComponentModel;

namespace SqlEditor.Databases
{
    public class Constraint : DatabaseObject
    {
        public Constraint(string name, DatabaseObject parent)
            : base(name, parent)
        {
        }

        public Constraint()
        {
        }

        public Table Table { get; set; }

        [DisplayName("Enforced")]
        public bool IsEnforced { get; set; }

        public string Type { get; set; }

        public override void Clear()
        {
        }
    }
}