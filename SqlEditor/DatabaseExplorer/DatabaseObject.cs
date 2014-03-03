using System;
using System.Collections.Generic;
using SqlEditor.Database;

namespace SqlEditor.DatabaseExplorer
{
//    public class DatabaseObject
//    {
//        public DatabaseObject(OracleSchema schema)
//        {
//            if (schema == null)
//            {
//                throw new ArgumentNullException("schema");
//            }
//            ParentSchema = schema;
//            Columns = new List<Column>();
//        }

//        public DatabaseObject(OracleSchema schema, string name)
//            : this(schema)
//        {
//            Name = name;
//        }

//        public DatabaseObject(OracleSchema schema, string name, string alias)
//            : this(schema, name)
//        {
//            Alias = alias;
//        }

//        public string Name { get; set; }
//        public string Alias { get; set; }
//        public OracleSchema ParentSchema { get; set; }

//        public string FullyQualifiedName
//        {
//            get { return ParentSchema.Name + "." + Name; }
//        }

//        public List<Column> Columns { get; set; }
//    }
}