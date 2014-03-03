using System.ComponentModel;

namespace SqlEditor.Databases
{
    public class DatabaseObjectWithColumns : DatabaseObject
    {
        public DatabaseObjectWithColumns(string name, DatabaseObject parent)
            : base(name, parent)
        {
            Initialize();
        }

        public DatabaseObjectWithColumns()
        {
            Initialize();
        }

        public BindingList<Column> Columns { get; set; }
        public BindingList<Column> PrimaryKeyColumns { get; set; }

        private void Initialize()
        {
            Columns = new BindingList<Column>();
            Columns.AddingNew += (sender, args) => ((Column) args.NewObject).Parent = this;
            PrimaryKeyColumns = new BindingList<Column>();
        }

        public override void Clear()
        {
            Columns.Clear();
            PrimaryKeyColumns.Clear();
        }
    }
}