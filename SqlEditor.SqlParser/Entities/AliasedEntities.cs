#region

using System;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class AliasedEntity : Expression
    {
        public AliasedEntity() : base(null)
        {
            Alias = new Alias(this);
        }

        public Alias Alias { get; set; }
    }

    public enum AliasType
    {
        None,
        Implicit,
        As,
        Equals
    }

    public class Alias : Expression
    {
        public Alias(Expression parent) : base(parent)
        {
            Type = AliasType.Implicit;
        }

        public string Name { get; set; }
        public AliasType Type { get; set; }

        public override string Value
        {
            get
            {
                string format = "";
                switch (Type)
                {
                    case AliasType.Implicit:
                        format = !String.IsNullOrEmpty(Name) ? String.Format(" {0}", Name) : "";
                        break;

                    case AliasType.Equals:
                        format = !String.IsNullOrEmpty(Name) ? String.Format("{0} = ", Name) : "";
                        break;

                    case AliasType.As:
                        format = String.Format(" AS {0}", Name);
                        break;
                }
                return format;
            }
            protected set { base.Value = value; }
        }

        public bool Equals(Alias other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Type, Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Alias)) return false;
            return Equals((Alias) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ Type.GetHashCode();
            }
        }
    }
}