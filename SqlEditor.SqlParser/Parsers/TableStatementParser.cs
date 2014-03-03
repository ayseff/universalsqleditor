using SqlEditor.SqlParser.Interfaces;

namespace SqlEditor.SqlParser.Parsers
{
    public abstract class TableStatementParser<T> : StatementParser<T> where T : class, IStatement
    {
        internal TableStatementParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }
    }
}