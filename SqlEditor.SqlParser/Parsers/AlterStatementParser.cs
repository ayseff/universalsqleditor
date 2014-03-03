using SqlEditor.SqlParser.Interfaces;

namespace SqlEditor.SqlParser.Parsers
{
    public class AlterStatementParser : IParser
    {
        private ITokenizer _tokenizer;

        /// <summary>
        ///   Initializes a new instance of the CreateStatementParser class.
        /// </summary>
        public AlterStatementParser(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        #region IParser Members

        public IStatement Execute()
        {
            IParser parser = null;

            if (_tokenizer.TokenEquals(Constants.Table))
                parser = new AlterTableStatementParser(_tokenizer);

            //if ( _tokenizer.TokenEquals( Constants.View ) )
            //    parser = new AlterViewStatementParser( _tokenizer );

            //if ( _tokenizer.TokenEquals( Constants.Procedure ) )
            //    parser = new AlterProcedureStatementParser( _tokenizer );

            //if ( _tokenizer.TokenEquals( Constants.Trigger ) )
            //    parser = new AlterTriggerStatementParser( _tokenizer );

            return parser != null ? parser.Execute() : null;
        }

        #endregion
    }
}