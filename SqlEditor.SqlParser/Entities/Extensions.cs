#region

using System;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    internal static class Extensions
    {
        public static string WithBrackets(this string field)
        {
            return String.Format("[{0}]", field.Trim(new[] {'[', ']'}));
        }
    }
}