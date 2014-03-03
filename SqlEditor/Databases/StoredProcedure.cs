namespace SqlEditor.Databases
{
    public class StoredProcedure : DatabaseObjectWithColumns
    {
        public string ObjectId { get; set; }
        public string Definition { get; set; }

        public StoredProcedure()
        {
        }

        public StoredProcedure(string name, DatabaseObject parent)
            : base(name, parent)
        {
        }
    }
}