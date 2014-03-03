#region

using SqlEditor.SqlParser.Entities;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    public class CreateViewStatementParser : StatementParser<CreateViewStatement>
    {
        internal CreateViewStatementParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        public override CreateViewStatement Execute()
        {
            _statement = new CreateViewStatement();
            _statement.Name = GetIdentifier();

            ExpectTokens(Constants.As, Constants.Select);

            SelectStatementParser parser = new SelectStatementParser(Tokenizer);
            _statement.SelectBlock = parser.Execute();

            return _statement;
        }
    }
}