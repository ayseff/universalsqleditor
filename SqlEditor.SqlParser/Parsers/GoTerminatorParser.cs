#region

using SqlEditor.SqlParser.Entities;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    public class GoTerminatorParser : StatementParser<GoTerminator>
    {
        public GoTerminatorParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        public override GoTerminator Execute()
        {
            return new GoTerminator();
        }
    }
}