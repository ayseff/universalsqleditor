#region

using System;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Exceptions;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    internal class InsertStatementParser : StatementParser<InsertStatement>
    {
        // INSERT [INTO] table_name [(column_list)] { 
        //      {
        //        VALUES ( { DEFAULT | NULL | expression }[,...n] )[,...n]
        //        | derived_table
        //        | execute_statement    
        //      }

        public InsertStatementParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        private void ProcessColumnList()
        {
            if (Tokenizer.IsNextToken(Constants.OpenBracket))
            {
                using (Tokenizer.ExpectBrackets())
                {
                    _statement.Columns = (GetIdentifierList());
                }
            }
        }

        private void ProcessValues()
        {
            do
            {
                using (Tokenizer.ExpectBrackets())
                {
                    _statement.Values.Add(GetIdentifierList());
                }
            } while (Tokenizer.TokenEquals(Constants.Comma));
        }

        private void ProcessSelect()
        {
            ReadNextToken();
            SelectExpression selectExpression = new SelectExpression();

            var parser = new SelectStatementParser(Tokenizer);
            _statement.SourceStatement = parser.Execute();
        }

        private void ProcessExec()
        {
            throw new NotImplementedException();
            //var parser = new ExecuteProcedureStatementParser( Tokenizer );
            //_statement.Procedure = parser.Execute<ExecuteProcuedreStatement>();
        }

        public override InsertStatement Execute()
        {
            _statement = new InsertStatement();

            try
            {
                if (Tokenizer.TokenEquals(Constants.Into))
                    _statement.HasInto = true;

                _statement.TableName = GetTableName();
                _statement.Tables.Add(new Table { Name = _statement.TableName });

                ProcessColumnList();

                if (Tokenizer.TokenEquals(Constants.Values))
                    ProcessValues();
                else if (Tokenizer.IsNextToken(Constants.Select))
                {
                    ProcessSelect();
                    foreach (var table in _statement.SourceStatement.Tables)
                    {
                        _statement.Tables.Add(table);
                    }
                }
                else
                    throw new SyntaxException(String.Format("Syntax error after 'INSERT INTO {0} '", _statement.TableName));

                ProcessTerminator();
                _statement.ParsedCompletely = true;
            }
            catch (Exception ex)
            {
                Log.Debug("Error while parsing Insert statement.");
                Log.Debug(ex.Message, ex);
            }

            return _statement;
        }
    }
}