#region

using System;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Tokenizer;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    public class DeleteStatementParser : CriteriaStatementParser<DeleteStatement>
    {
        public DeleteStatementParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        public override DeleteStatement Execute()
        {
            _statement = new DeleteStatement();

            try
            {
                if (!Tokenizer.IsNextToken(Constants.From))
                {
                    _statement.TableName = GetTableName();
                    Table table = new Table { Name = _statement.TableName };
                    _statement.Tables.Add(table);
                    Alias alias = new Alias(null);
                    if (Tokenizer.IsNextToken(Constants.As))
                    {
                        alias.Type = AliasType.As;
                        Tokenizer.ReadNextToken();
                    }
                    if (!Tokenizer.IsNextToken(Constants.OpenBracket) &&
                        (alias.Type != AliasType.Implicit || !Tokenizer.IsNextToken(Constants.Set)))
                    {
                        if (Tokenizer.HasMoreTokens && Tokenizer.Current.IsTypeIn(TokenType.Alpha, TokenType.AlphaNumeric, TokenType.BlockedText,
                                                           TokenType.SingleQuote))
                        {
                            alias.Name = CurrentToken;
                            table.Alias = alias;
                            ReadNextToken();
                        }
                    }
                }

                bool quoteOn = false;
                while (Tokenizer.HasMoreTokens && (quoteOn || (!quoteOn && !HasTerminator())))
                {
                    if (Tokenizer.IsNextToken("\"", "'"))
                    {
                        quoteOn = !quoteOn;
                    }

                    if (quoteOn)
                    {
                        Tokenizer.ReadNextToken();
                    }
                    else if (Tokenizer.Current != Constants.From)
                    {
                        Tokenizer.ReadNextToken();
                    }
                    else
                    {
                        ProcessFrom();
                    }
                }
                _statement.ParsedCompletely = true;
            }
            catch (Exception ex)
            {
                Log.Debug("Error parsing INSERT statement.");
                Log.Debug(ex.Message, ex);
            }



            return _statement;
        }
    }
}