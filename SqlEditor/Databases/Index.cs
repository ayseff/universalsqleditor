using System.ComponentModel;

namespace SqlEditor.Databases
{
    public class Index : DatabaseObjectWithColumns
    {
        public Index(string name, DatabaseObject parent)
            : base(name, parent)
        {
        }

        public Index()
        {
        }

        public Table Table { get; set; }

        [DisplayName("Unique")]
        public bool IsUnique { get; set; }
    }
}