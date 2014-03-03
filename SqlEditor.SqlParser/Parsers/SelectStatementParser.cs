#region

using System.Reflection;
using SqlEditor.SqlParser.Entities;
using log4net;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    public class SelectStatementParser : CriteriaStatementParser<SelectStatement>
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SelectStatementParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        public override SelectStatement Execute()
        {
            _statement = new SelectStatement();

            try
            {
                var quoteOn = false;
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
            catch (System.Exception ex) 
            {
                
                _log.Debug("Error parsing SQL statement.");
                _log.Debug(ex.Message, ex);
            }
            return _statement;
        }
    }
}