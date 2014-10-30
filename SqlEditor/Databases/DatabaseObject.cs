using System;
using System.ComponentModel;
using Utilities.Text;

namespace SqlEditor.Databases
{
    public abstract class DatabaseObject
    {
        private string _name;
        private object _id;

        protected DatabaseObject()
        {
        }

        protected DatabaseObject(string name, DatabaseObject parent)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
            Parent = parent;
        }

        protected DatabaseObject(string name, string displayName, DatabaseObject parent)
            : this(name, parent)
        {
            if (displayName == null) throw new ArgumentNullException("displayName");
        }

        [Browsable(false)]
        public DatabaseObject Parent { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (_name != null)
                {
                    _name = _name.Trim();
                }
            }
        }

        public object Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Browsable(false)]
        public virtual string DisplayName { get { return Name.ToUpper();  } }

        [Browsable(false)]
        public string FullyQualifiedName
        {
            get
            {
                if (Parent != null && !Parent.Name.IsNullEmptyOrWhitespace())
                {
                    return Parent.FullyQualifiedName + "." + Name;
                }
                return Name;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract void Clear();

        public string GetDatabaseName()
        {
            string databaseName = null;
            if (Parent != null &&
                Parent.Parent != null)
            {
                databaseName = Parent.Parent.Name;
            }
            return databaseName;
        }

        public string GetSchemaName()
        {
            string schemaName = null;
            if (Parent != null)
            {
                schemaName = Parent.Name;
            }
            return schemaName;
        }
    }
}