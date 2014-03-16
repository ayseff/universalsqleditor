using System.ComponentModel;
using SqlEditor.Database;

namespace SqlEditor.Databases
{
    public class Schema : DatabaseObject
    {
        public Schema()
        {
            Initialize();
        }

        public Schema(string name) : base(name, null)
        {
            Initialize();
        }

        public Schema(string name, string displayName)
            : base(name, displayName, null)
        {
            Initialize();
        }

        public BindingList<Table> Tables { get; set; }
        public BindingList<View> Views { get; set; }
        public BindingList<MaterializedView> MaterializedViews { get; set; }
        public BindingList<Index> Indexes { get; set; }
        public BindingList<Sequence> Sequences { get; set; }
        public BindingList<Synonym> Synonyms { get; set; }
        public BindingList<Trigger> Triggers { get; set; }
        public BindingList<StoredProcedure> StoredProcedures { get; set; }

        private void Initialize()
        {
            Tables = new BindingList<Table>();
            Tables.AddingNew += AddingNew;
            Views = new BindingList<View>();
            Views.AddingNew += AddingNew;
            MaterializedViews = new BindingList<MaterializedView>();
            MaterializedViews.AddingNew += AddingNew;
            Indexes = new BindingList<Index>();
            Indexes.AddingNew += AddingNew;
            Sequences = new BindingList<Sequence>();
            Sequences.AddingNew += AddingNew;
            Synonyms = new BindingList<Synonym>();
            Synonyms.AddingNew += AddingNew;
            Triggers = new BindingList<Trigger>();
            Triggers.AddingNew += AddingNew;
            StoredProcedures = new BindingList<StoredProcedure>();
            StoredProcedures.AddingNew += AddingNew;
        }

        private void AddingNew(object sender, AddingNewEventArgs e)
        {
            ((DatabaseObject) e.NewObject).Parent = this;
        }

        public override void Clear()
        {
            Tables.Clear();
            Views.Clear();
            MaterializedViews.Clear();
            Indexes.Clear();
            Sequences.Clear();
            Synonyms.Clear();
            Triggers.Clear();
            StoredProcedures.Clear();
        }
    }
}