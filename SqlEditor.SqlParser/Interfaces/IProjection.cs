#region

using System.Collections.Generic;
using SqlEditor.SqlParser.Entities;

#endregion

namespace SqlEditor.SqlParser
{
    public interface IProjection
    {
        List<Field> Fields { get; }
    }
}