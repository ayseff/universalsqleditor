namespace SqlEditor.SqlParser.Expressions
{
    public interface IInlineFormattable
    {
        bool CanInline { get; }
    }
}