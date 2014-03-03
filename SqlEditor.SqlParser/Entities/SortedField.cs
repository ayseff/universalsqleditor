#region

using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public enum SortOrder
    {
        Implicit,
        Ascending,
        Descending
    }

    public class SortedField : Field
    {
        private Dictionary<SortOrder, string> _map;

        /// <summary>
        ///   Initializes a new instance of the SortedField class.
        /// </summary>
        /// <param name = "map"></param>
        public SortedField()
        {
            _map = new Dictionary<SortOrder, string>
                       {
                           {SortOrder.Implicit, ""},
                           {SortOrder.Ascending, " ASC"},
                           {SortOrder.Descending, " DESC"}
                       };
        }

        public SortOrder SortOrder { get; set; }

        public override string Value
        {
            get { return base.Value + _map[SortOrder]; }
            protected set { base.Value = value; }
        }
    }
}