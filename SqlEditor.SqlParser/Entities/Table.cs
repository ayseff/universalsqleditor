#region

using System;
using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class Table : AliasedEntity, ITableHints
    {
        /// <summary>
        ///   Initializes a new instance of the Table class.
        /// </summary>
        public Table()
        {
            Joins = new List<Join>();
            TableHints = new List<TableHint>();
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                {
                    _name = value;
                }
                else
                {
                    _name = value.Trim().ToUpper();
                    if (_name.IndexOf(".") >= 0)
                    {
                        string[] dorSplits = _name.Split('.');
                        Schema = dorSplits[0].Trim();
                        _name = dorSplits[1].Trim();
                    }
                }
            }
        }

        private string _schema;
        public string Schema 
        {
            get { return _schema; }
            set
            {
                if (value == null)
                {
                    _schema = value;
                }
                else
                {
                    _schema = value.Trim().ToUpper();
                }
            }
        }
        public List<Join> Joins { get; set; }

        public override string Value
        {
            get { return Alias != null ? Name : String.Format("{0} ({1})", Name, Alias == null ? string.Empty : Alias.Name); }
        }

        #region ITableHints Members

        public List<TableHint> TableHints { get; set; }

        #endregion
    }

    public class DerivedTable : Table
    {
        public SelectStatement SelectStatement { get; set; }

        public override string Value
        {
            get { return "(" + SelectStatement + ")"; }
        }
    }
}