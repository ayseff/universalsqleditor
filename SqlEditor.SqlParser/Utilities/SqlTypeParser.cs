#region

using System;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Parsers;

#endregion

namespace SqlEditor.SqlParser
{
    public class SqlTypeParser : CustomParser
    {
        public SqlTypeParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        public SqlType Execute()
        {
            string identifier = GetIdentifier();
            SqlType result;
            if (identifier == Constants.As)
            {
                result = null;
            }
            else
            {
                result = new SqlType(identifier);

                if (!Tokenizer.IsNextToken(Constants.OpenBracket))
                    return result;

                using (Tokenizer.ExpectBrackets())
                {
                    string token = CurrentToken;
                    ReadNextToken();
                    result.Max = (String.Compare(token, "MAX", true) == 0);

                    if (!result.Max)
                    {
                        result.Length = Int32.Parse(token);

                        if (Tokenizer.TokenEquals(Constants.Comma))
                        {
                            result.Scale = Int32.Parse(CurrentToken);
                            ReadNextToken();
                        }
                    }
                }
            }
            return result;
        }
    }
}