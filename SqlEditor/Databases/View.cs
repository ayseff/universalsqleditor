using SqlEditor.Databases;

namespace SqlEditor.Database
{
    public class View : DatabaseObjectWithColumns
    {
        public View(string name, DatabaseObject parent)
            : base(name, parent)
        {
        }

        public View()
        {
        }
    }
}