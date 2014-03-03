namespace SqlEditor.SqlParser.Entities
{
    public class GrantStatement : Statement
    {
        public string Operation { get; set; }
        public string TableName { get; set; }
        public string Grantee { get; set; }
    }
}