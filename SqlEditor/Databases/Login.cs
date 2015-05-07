namespace SqlEditor.Databases
{
    public class Login : DatabaseObject
    {
        public Login()
        {
        }

        public Login(string name) : base(name, null)
        {
        }

        public Login(string name, string displayName)
            : base(name, displayName, null)
        {
        }

        public override void Clear()
        {
        }
    }
}