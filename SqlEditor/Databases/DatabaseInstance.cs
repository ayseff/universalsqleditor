using System.Collections.Generic;
using System.ComponentModel;

namespace SqlEditor.Databases
{
    public class DatabaseInstance : DatabaseObject
    {
        private readonly BindingList<Schema> _schemas = new BindingList<Schema>();

        public IList<Schema> Schemas
        {
            get { return _schemas; }
        }

        public DatabaseInstance()
        {
            AttachItemAddedEvent();
        }

        public DatabaseInstance(string name)
            : base(name, null)
        {
            AttachItemAddedEvent();
        }

        public DatabaseInstance(string name, string displayName)
            : base(name, displayName, null)
        {
            AttachItemAddedEvent();
        }

        public override void Clear()
        {
            Schemas.Clear();
        }

        private void AttachItemAddedEvent()
        {
            _schemas.ListChanged += (sender, args) =>
            {
                if (args.ListChangedType == ListChangedType.ItemAdded)
                {
                    var itemAdded = _schemas[args.NewIndex] as DatabaseObject;
                    if (itemAdded != null)
                    {
                        itemAdded.Parent = this;
                    }
                }
            };
        }
    }
}