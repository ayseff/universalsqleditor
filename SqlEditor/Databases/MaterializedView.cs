using SqlEditor.Databases;

namespace SqlEditor.Database
{
    public class MaterializedView : DatabaseObjectWithColumns
    {
        public MaterializedView(string name, DatabaseObject parent)
            : base(name, parent)
        {
        }

        public MaterializedView()
        {
        }
    }
}