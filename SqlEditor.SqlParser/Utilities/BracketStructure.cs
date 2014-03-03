#region

using System;

#endregion

namespace SqlEditor.SqlParser
{
    public class BracketStructure : IDisposable
    {
        private ITokenizer _tokenizer;

        public BracketStructure(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _tokenizer.ExpectToken(Constants.OpenBracket);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _tokenizer.ExpectToken(Constants.CloseBracket);
        }

        #endregion
    }
}