namespace SqlEditor.Databases
{
    public class Function : StoredProcedure
    {
        public Function(string name, DatabaseObject parent) : base(name, parent)
        {
        }

        public Function()
        {
        }
    }
}