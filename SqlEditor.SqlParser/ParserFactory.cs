#region

using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.SqlParser.Exceptions;
using SqlEditor.SqlParser.Interfaces;
using SqlEditor.SqlParser.Parsers;
using SqlEditor.SqlParser.Tokenizer;
using log4net;

#endregion

namespace SqlEditor.SqlParser
{
    public class ParserFactory
    {
        private static ILog _log;
        protected static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof (ParserFactory));
                }
                return _log;
            }
        }

        private static readonly Dictionary<string, Type> Parsers;

        /// <summary>
        ///   Initializes a new instance of the ParserFactory class.
        /// </summary>
        static ParserFactory()
        {
            Parsers = new Dictionary<string, Type>
                           {
                               {Constants.Select, typeof (SelectStatementParser)},
                               {Constants.Insert, typeof (InsertStatementParser)},
                               {Constants.Update, typeof (UpdateStatementParser)},
                               {Constants.Delete, typeof (DeleteStatementParser)},
                               {Constants.Grant, typeof (GrantStatementParser)},
                               {Constants.Go, typeof (GoTerminatorParser)},
                               {Constants.Create, typeof (CreateStatementParser)},
                               {Constants.Alter, typeof (AlterStatementParser)},
                               {Constants.Declare, typeof (DeclareStatementParser)},
                               {Constants.If, typeof (IfStatementParser)},
                               {Constants.Begin, typeof (BeginStatementParser)},
                               {Constants.Commit, typeof (CommitStatementParser)},
                               {Constants.Rollback, typeof (RollbackStatementParser)},
                               {Constants.Set, typeof (SetStatementParser)},
                               {Constants.Exec, typeof (ExecuteSqlStatementParser)}
                           };
        }

        internal static IParser GetParser(ITokenizer tokenizer)
        {
            // this is a quick and dirty service locator that maps tokens to parsers
            Type parserType;
            if (Parsers.TryGetValue(tokenizer.Current.Value.ToUpper(), out parserType))
            {
                tokenizer.ReadNextToken();

                object instance = Activator.CreateInstance(parserType, tokenizer);
                return (IParser) instance;
            }
            return null;
        }

        /// <summary>
        ///   This method is used if you know what type will be returned from the parser
        ///   - only use it if 100% confident, otherwise you will get a null reference
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "sql"></param>
        /// <returns></returns>
        public static List<T> Execute<T>(string sql) where T : class, IStatement
        {
            List<IStatement> list = Execute(sql);
            return list.Cast<T>().ToList<T>();
        }

        public static List<IStatement> Execute(ITokenizer tokenizer, bool ensureParserIsFound)
        {
            var result = new List<IStatement>();

            try
            {
                if (tokenizer.Current == (Token)null)
                    tokenizer.ReadNextToken();

                while (tokenizer.HasMoreTokens)
                {
                    IParser parser = GetParser(tokenizer);

                    if (parser != null)
                        result.Add(parser.Execute());
                    else if (ensureParserIsFound)
                        throw new ParserNotImplementedException(
                            "No parser exists for statement type: " + tokenizer.Current.Value
                            );
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Unable to parse statement.");
                Log.Debug(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        ///   This will parse any statement, and return only the interface (IStatement)
        /// </summary>
        /// <param name = "sql"></param>
        /// <returns></returns>
        public static List<IStatement> Execute(string sql)
        {
            using (SqlTokenizer sqlTokenizer = new SqlTokenizer(sql))
            {
                return Execute(sqlTokenizer, true);
            }
        }
    }
}