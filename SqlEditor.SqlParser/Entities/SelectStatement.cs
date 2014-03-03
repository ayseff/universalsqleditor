#region

using System.Collections.Generic;
using System.Linq;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public enum SetType
    {
        None,
        Union,
        UnionAll,
        Intersect,
        Except
    }


    public class SelectStatement : ProjectionStatement
    {
        private const int MaxInlineColumns = 1;

        public SelectStatement()
        {
            Distinct = false;
            Top = null;
            OrderBy = new List<Field>();
            GroupBy = new List<Field>();
        }

        public bool Distinct { get; set; }
        public Top Top { get; set; }
        public string Into { get; set; }
        public List<Field> OrderBy { get; set; }
        public List<Field> GroupBy { get; set; }
        public Expression Having { get; set; }
        public SetOperation SetOperation { get; set; }

        public bool CanInLine()
        {
            return Fields.Count <= MaxInlineColumns
                   &&
                   From.Count == 1 &&
                   !From.First().Joins.Any()
                   && (Where == null || Where.CanInline)
                   && (GroupBy == null || !GroupBy.Any())
                   && (OrderBy == null || !OrderBy.Any());
        }
    }
}