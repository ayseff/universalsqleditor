using System;
using System.ComponentModel;
using Utilities.Text;

namespace SqlEditor.Databases
{
    public class Column : DatabaseObject
    {
        public Column(string name, DatabaseObject parent)
            : base(name, parent)
        {
        }

        public Column()
        {
        }

        [Browsable(false)]
        public override string DisplayName
        {
            get
            {
                var name = Name.ToUpper();
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
                    if (Nullable)
                    {
                        name += " NULL";
                    }
                    else
                    {
                        name += " NOT NULL";
                    }
                    name += ")";
                }
                return name;
            }
        }

        public override void Clear()
        {
            
        }

        [DisplayName("Data Type")]
        public string DataType { get; set; }
        [DisplayName("Character Length")]
        public long? CharacterLength { get; set; }
        [DisplayName("Data Precision")]
        public int? DataPrecision { get; set; }
        [DisplayName("Data Scale")]
        public int? DataScale { get; set; }
        [DisplayName("Nullable")]
        public bool Nullable { get; set; }
        [DisplayName("Ordinal Position")]
        public int OrdinalPosition { get; set; }
    }
}