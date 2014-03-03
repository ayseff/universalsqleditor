#region

using System;
using System.Collections.Generic;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Exceptions;
using SqlEditor.SqlParser.Expressions;
using SqlEditor.SqlParser.Tokenizer;
using log4net;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    public abstract class CustomParser
    {
        private static ILog _log;
        protected static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof(ParserFactory));
                }
                return _log;
            }
        }

        public CustomParser(ITokenizer tokenizer)
        {
            Tokenizer = tokenizer;
        }

        protected string CurrentToken
        {
            get { return Tokenizer.Current.Value; }
        }

        protected ITokenizer Tokenizer { get; private set; }

        protected void ExpectToken(string token)
        {
            if (CurrentToken.ToLower() != token.ToLower())
                throw new ExpectedTokenNotFoundException(token, CurrentToken, Tokenizer.Position);
            else
                ReadNextToken();
        }

        protected void ExpectTokens(params string[] tokens)
        {
            foreach (string token in tokens)
                ExpectToken(token);
        }

        protected void ReadNextToken()
        {
            Tokenizer.ReadNextToken();
        }

        protected Expression ProcessExpression()
        {
            var parser = new ExpressionParser(Tokenizer);
            return parser.Execute();
        }

        private string GetOperator()
        {
            if (Tokenizer.IsNextToken("=", ">=", "<=", "!=", "<>", "IN", "ANY", "LIKE"))
            {
                string token = Tokenizer.Current.Value;
                ReadNextToken();
                return token;
            }
            else
                throw new ExpectedTokenNotFoundException(
                    "'=', '>=', '<=', '!=', '<>', 'IN', 'ANY', 'LIKE'",
                    CurrentToken,
                    Tokenizer.Position
                    );
        }

        protected CriteriaExpression ProcessCriteriaExpression(Expression parent)
        {
            CriteriaExpression expression = new CriteriaExpression(parent);
            expression.Left = ProcessExpression();
            expression.Operator = GetOperator();
            expression.Right = ProcessExpression();

            return expression;
        }

        protected string GetIdentifier()
        {
            if (!Tokenizer.HasMoreTokens)
                throw new SyntaxException("Identifier expected");

            string identifier;
            switch (Tokenizer.Current.Type)
            {
                case TokenType.SingleQuote:
                    var parser = new ExpressionParser(Tokenizer);
                    var expression = parser.Execute();

                    if (!(expression is StringExpression))
                        throw new SyntaxException("Transaction name must be a string");

                    identifier = expression.Value;
                    break;

                default:
                    identifier = CurrentToken;
                    ReadNextToken();
                    break;
            }

            return identifier;
        }

        protected List<string> GetIdentifierList()
        {
            List<string> identifiers = new List<string>();
            do
            {
                identifiers.Add(GetIdentifier());
            } while (Tokenizer.TokenEquals(Constants.Comma));

            return identifiers;
        }

        protected string GetDotNotationIdentifier()
        {
            string token;
            token = "";

            do
            {
                token += (token != "" ? Constants.Dot : "") + GetIdentifier();
            } while (Tokenizer.TokenEquals(Constants.Dot));
            return token;
        }

        protected bool HasTerminator()
        {
            bool result = Tokenizer.IsNextToken(Constants.SemiColon, Constants.Slash);
            if (result)
                Tokenizer.ReadNextToken();
            return result;
        }

        protected Top GetTop()
        {
            // consume 'TOP' token first
            Tokenizer.ExpectToken(Constants.Top);

            Top top;
            if (Tokenizer.IsNextToken(Constants.OpenBracket))
            {
                using (Tokenizer.ExpectBrackets())
                {
                    var parser = new ExpressionParser(Tokenizer);
                    var expression = parser.Execute();
                    if (expression != null)
                    {
                        top = new Top(expression, true);
                        return top;
                    }
                    else
                        throw new SyntaxException("TOP clause requires an expression");
                }
            }
            else
            {
                if (Tokenizer.Current.Type != TokenType.Numeric || Tokenizer.Current.Value.Contains("."))
                    throw new SyntaxException(String.Format("Expected integer but found: '{0}'", Tokenizer.Current.Value));

                top = new Top(new StringExpression(Tokenizer.Current.Value, null), false);
                ReadNextToken();
            }

            if (Tokenizer.TokenEquals(Constants.Percent))
                top.Percent = true;

            return top;
        }
    }
}