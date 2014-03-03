#region

using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class ProjectionStatement : CustomStatement
    {
        /// <summary>
        ///   Initializes a new instance of the BaseStatement class.
        /// </summary>
        public ProjectionStatement()
        {
            Fields = new List<Field>();
        }

        public List<Field> Fields { get; set; }
    }
}