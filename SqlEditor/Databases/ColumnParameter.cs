using System.ComponentModel;
using Utilities.Text;

namespace SqlEditor.Databases
{
    public class ColumnParameter : Column
    {
        public string Direction { get; set; }
        
        public ColumnParameter()
        {
        }

        public ColumnParameter(string name, DatabaseObject parent) : base(name, parent)
        {
        }
        
        public override string DisplayName
        {
            get
            {
                var name = Direction + " " + Name.ToUpper();
                if (!DataType.IsNullEmptyOrWhitespace())
                {
                    name += " (" + DataType;
                    if (DataPrecision.GetValueOrDefault(0) > 0)
                    {
                        name += " (" + DataPrecision.Value;
                        if (DataScale.GetValueOrDefault(0) > 0)
                        {
                            name += ", " + DataScale.Value;
                        }
                        name += ")";
                    }
                    else if (CharacterLength.GetValueOrDefault(0) > 0)
                    {
                        name += " (" + CharacterLength.Value + ")";
                    }
                    
                    name += ")";
                }
                return name;
            }
        }

        public override string ToString()
        {
            return Direction + " " + base.ToString();
        }
    }
}