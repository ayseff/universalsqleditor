#region

using System.Linq;
using SqlEditor.SqlParser.Tokenizer;

#endregion

namespace SqlEditor.SqlParser
{
    public class SqlTokenizer : RegexTokenizer, ITokenizer
    {
        public SqlTokenizer(string input) : base(input)
        {
            TokenDefinitions.AddRange(
                new[]
                    {
                        new TokenDefinition(TokenType.WhiteSpace, true, @"^\s+$"),
                        new TokenDefinition(TokenType.Alpha, false, @"^[a-zA-Z_]+$"),
                        new TokenDefinition(TokenType.Variable, false, @"^(:|@{1,2}|#{1,2})[A-Za-z_]+[[A-Za-z_0-9]*$"),
                        new TokenDefinition(TokenType.AlphaNumeric, false, @"^[A-Za-z_]+\w+$"),
                        new TokenDefinition(TokenType.Numeric, false, @"^[-]?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$"),
                        new TokenDefinition(TokenType.OpenBracket, false, @"^\($"),
                        new TokenDefinition(TokenType.CloseBracket, false, @"^\)$"),
                        new TokenDefinition(TokenType.SingleQuote, false, @"^'$"),
                        new TokenDefinition(TokenType.DoubleQuote, false, @"^""$"),
                        new TokenDefinition(TokenType.BlockedText, false, @"^\[.*\]$"),
                        new TokenDefinition(TokenType.Symbol, false, @"^[,.;:]$"),
                        new TokenDefinition(TokenType.Operator, false, @"^([\+\-\*/\^\%]|<?>?|[!<>]?=)$"),
                        new TokenDefinition(TokenType.InLineComment, true, @"^\-\-[^\n\r]*$"),
                        new TokenDefinition(TokenType.MultiLineComment, true, @"\/\*(\s|.)*?(\*\/)|\/\*(\s|.)*"),
                    }
                );
        }

        public bool SkipComments
        {
            set
            {
                TokenDefinitions
                    .Where(tdef => tdef.Type == TokenType.InLineComment || tdef.Type == TokenType.MultiLineComment)
                    .ToList()
                    .ForEach(tdef => tdef.Skip = value);
            }
        }

        #region ITokenizer Members

        public bool SkipWhiteSpace
        {
            set
            {
                TokenDefinitions
                    .Where(tdef => tdef.Type == TokenType.WhiteSpace)
                    .ToList()
                    .ForEach(tdef => tdef.Skip = value);
            }
        }

        #endregion
    }
}