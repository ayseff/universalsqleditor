namespace SqlEditor.SqlParser.Entities
{
    public class DeleteStatement : InsertDeleteStatementBase
    {
        public Top Top { get; set; }
        public string TableName { get; set; }
    }
}