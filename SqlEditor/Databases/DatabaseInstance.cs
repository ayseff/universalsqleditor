namespace SqlEditor.Databases
{
    public class DatabaseInstance : DatabaseObject
    {
        public DatabaseInstance()
        {
        }

        public DatabaseInstance(string name)
            : base(name, null)
        {
        }

        public DatabaseInstance(string name, string displayName)
            : base(name, displayName, null)
        {
        }

        public override void Clear()
        {
        }
    }
}