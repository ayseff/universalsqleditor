namespace SqlEditor.SqlParser.Entities
{
    public class SetOperation
    {
        public SelectStatement Statement { get; set; }
        public SetType Type { get; set; }
    }
}