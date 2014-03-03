#region

using System.Linq;
using SqlEditor.SqlParser.Exceptions;
using SqlEditor.SqlParser.Tokenizer;

#endregion

namespace SqlEditor.SqlParser
{
    public abstract class CustomTokenizer
    {
        //private abstract void InternalSetSkipWhiteSpace();

        //public bool SkipWhiteSpace
        //{
        //    set { InternalSetSkipWhiteSpace(); }
        //}

        public virtual Token Current
        {
            get { return new Token(); }
        }

        public Position Position { get; protected set; }

        public virtual bool HasMoreTokens
        {
            get { return false; }
        }

        public virtual bool IsNextToken(params string[] tokenSet)
        {
            return tokenSet.Any(token => Current == token);
        }
        
        public bool TokenEquals(string value)
        {
            bool areEqual = Current == value;
            if (areEqual)
                ReadNextToken();

            return areEqual;
        }


        public virtual void ReadNextToken()
        {
            // do nothing
        }

        /// <summary>
        ///   Verify current token matches expected token. Read next token if successful.
        /// </summary>
        /// <param name = "token">Expected token</param>
        /// <exception cref = "ExpectedTokenNotFoundException">current token did not match</exception>
        public void ExpectToken(string token)
        {
            if (Current != token)
                throw new ExpectedTokenNotFoundException(token, Current.Value, Position);
            else
                ReadNextToken();
        }

        /// <summary>
        ///   Verify current tokens match expected tokens. Read next token if successful.
        /// </summary>
        /// <param name = "tokens">Expected tokens</param>
        /// <exception cref = "ExpectedTokenNotFoundException">current token did not match</exception>
        public void ExpectTokens(string[] tokens)
        {
            foreach (string token in tokens)
                ExpectToken(token);
        }

        public BracketStructure ExpectBrackets()
        {
            return new BracketStructure(this as ITokenizer);
        }
    }
}