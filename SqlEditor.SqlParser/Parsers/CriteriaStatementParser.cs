#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Exceptions;
using SqlEditor.SqlParser.Expressions;
using SqlEditor.SqlParser.Tokenizer;

#endregion

namespace SqlEditor.SqlParser.Parsers
{
    public abstract class CriteriaStatementParser<T> : StatementParser<T> where T : CustomStatement
    {
        protected string[] FieldTerminatorSet = {
                                                    Constants.From, Constants.Comma, Constants.Having, Constants.Go,
                                                    Constants.SemiColon, Constants.End, Constants.Into
                                                };

        protected string[] FromTerminatorSet = {
                                                   Constants.Inner, Constants.Join, Constants.Left, Constants.Right,
                                                   Constants.Full, Constants.Comma, Constants.CloseBracket, Constants.Order
                                                   , Constants.Group, Constants.Where
                                               };

        protected CriteriaStatementParser(ITokenizer tokenizer) : base(tokenizer)
        {
        }

        private SortedField GetOrderByField(Expression token)
        {
            SortOrder sortOrder = SortOrder.Implicit;
            if (Tokenizer.TokenEquals("ASC"))
                sortOrder = SortOrder.Ascending;
            if (Tokenizer.TokenEquals("DESC"))
                sortOrder = SortOrder.Descending;
            return new SortedField {Expression = token, SortOrder = sortOrder};
        }

        private Field GetSelectField(Expression token)
        {
            Expression expression;
            Alias alias = new Alias(token);
            if (token is CriteriaExpression)
            {
                // this handles the non-standard syntax of: Alias = Expression
                CriteriaExpression criteriaExpression = (CriteriaExpression) token;
                alias.Name = criteriaExpression.Left.Value;
                alias.Type = AliasType.Equals;
                expression = criteriaExpression.Right;
            }
            else if (Tokenizer.TokenEquals(Constants.As))
            {
                alias.Name = CurrentToken;
                alias.Type = AliasType.As;
                expression = token;
                ReadNextToken();
            }
            else
            {
                if (!Tokenizer.IsNextToken(FieldTerminatorSet))
                {
                    if (Tokenizer.HasMoreTokens)
                    {
                        alias.Name = CurrentToken;
                        alias.Type = AliasType.Implicit;
                        ReadNextToken();
                    }
                    else
                        alias.Type = AliasType.None;
                }
                expression = token;
            }

            return new Field {Expression = expression, Alias = alias};
        }

        private Field GetUpdateField(Expression token)
        {
            Alias alias = new Alias(token);
            Expression expression;

            if (token is CriteriaExpression)
            {
                CriteriaExpression criteriaExpression = (CriteriaExpression) token;
                alias.Name = criteriaExpression.Left.Value;
                alias.Type = AliasType.Equals;
                expression = criteriaExpression.Right;
            }
            else
                throw new SyntaxException(String.Format("Expected field assignment at {0}", Tokenizer.Position));

            return new Field {Expression = expression, Alias = alias};
        }

        private void ProcessField(FieldType fieldType, List<Field> fieldList)
        {
            Expression token = ProcessExpression();
            Field field = null;
            switch (fieldType)
            {
                case FieldType.Select:
                    field = GetSelectField(token);
                    break;
                case FieldType.Update:
                    field = GetUpdateField(token);
                    break;
                case FieldType.OrderBy:
                    field = GetOrderByField(token);
                    break;
                case FieldType.GroupBy:
                    field = new Field {Expression = token};
                    break;
            }
            if (field != null)
                fieldList.Add(field);
        }

        protected void ProcessFields(FieldType fieldType, List<Field> fieldList)
        {
            do
                ProcessField(fieldType, fieldList); while (Tokenizer.TokenEquals(Constants.Comma));

            if (fieldList.Count == 0)
                throw new SyntaxException("field list can not be empty");
        }

        private JoinType? GetJoinType()
        {
            JoinType? joinType = null;
            if (Tokenizer.TokenEquals(Constants.Inner))
                joinType = JoinType.InnerJoin;
            else if (Tokenizer.IsNextToken(Constants.Join))
                // don't consume - it is checked after here
                joinType = JoinType.Join;
            else if (Tokenizer.TokenEquals(Constants.Full))
            {
                joinType = JoinType.FullJoin;
                if (Tokenizer.TokenEquals(Constants.Outer))
                    joinType = JoinType.FullOuterJoin;
            }
            else if (Tokenizer.TokenEquals(Constants.Left))
            {
                joinType = JoinType.LeftJoin;
                if (Tokenizer.TokenEquals(Constants.Outer))
                    joinType = JoinType.LeftOuterJoin;
            }
            else if (Tokenizer.TokenEquals(Constants.Right))
            {
                joinType = JoinType.RightJoin;
                if (Tokenizer.TokenEquals(Constants.Outer))
                    joinType = JoinType.RightOuterJoin;
            }

            return joinType;
        }

        protected void ProcessFrom()
        {
            if (!Tokenizer.TokenEquals(Constants.From) || !Tokenizer.HasMoreTokens)
                return;

            do
            {
                if (Tokenizer.IsNextToken(FromTerminatorSet))
                {
                    return;
                }
                Table table;
                if (Tokenizer.IsNextToken(Constants.OpenBracket))
                    return;
                else
                {
                    table = new Table();
                    table.Name = GetTableName();
                }
                _statement.Tables.Add(table);
                _statement.From.Add(table);

                // TODO: This needs to be changed to test Tokenizer.Token.Current.TokenType for TokenType.Keyword
                // if a new statement is initiated here, do not process the alias
                if (Tokenizer.IsNextToken(
                    Constants.SemiColon, Constants.Go, Constants.Select, Constants.Insert,
                    Constants.Update, Constants.Delete, Constants.Create, Constants.Alter,
                    Constants.Union, Constants.Else, Constants.Commit, Constants.Rollback,
                    Constants.End, Constants.Except, Constants.Intersect, Constants.Slash
                    )
                    )
                    return;

                Alias alias = new Alias(null);
                if (Tokenizer.IsNextToken(Constants.As))
                {
                    alias.Type = AliasType.As;
                    Tokenizer.ReadNextToken();
                }
                if (!Tokenizer.IsNextToken(Constants.OpenBracket) &&
                    (alias.Type != AliasType.Implicit || !Tokenizer.IsNextToken(FromTerminatorSet)))
                {
                    if (Tokenizer.HasMoreTokens)
                    {
                        if (!Tokenizer.Current.IsTypeIn(
                            TokenType.Alpha, TokenType.AlphaNumeric, TokenType.BlockedText, TokenType.SingleQuote
                                 )
                            )
                            throw new SyntaxException(String.Format("Incorrect syntax near '{0}'", CurrentToken));

                        alias.Name = CurrentToken;
                        table.Alias = alias;
                        ReadNextToken();
                    }
                }
                ProcessJoins(table);
            } while (Tokenizer.HasMoreTokens && Tokenizer.TokenEquals(Constants.Comma));
        }

        private void ProcessJoins(Table table)
        {
            if (!Tokenizer.HasMoreTokens)
                return;
            do
            {
                JoinType? joinType = GetJoinType();
                if (joinType == null)
                    return;

                ExpectToken(Constants.Join);

                Join join;
                Table joinTable = null;
                if (Tokenizer.IsNextToken(Constants.OpenBracket))
                    using (Tokenizer.ExpectBrackets())
                    {
                        join = new DerivedJoin {Type = joinType.Value};
                        Tokenizer.ExpectToken(Constants.Select);
                        var parser = new SelectStatementParser(Tokenizer);
                        ((DerivedJoin) join).SelectStatement = parser.Execute();
                    }
                else
                {
                    join = new Join {Type = joinType.Value};
                    join.Name = GetTableName();
                    joinTable = new Table {Name = join.Name};
                    _statement.Tables.Add(joinTable);
                }

                Debug.Assert(join != null);

                Alias alias = new Alias(null);
                if (Tokenizer.IsNextToken(Constants.As))
                {
                    alias.Type = AliasType.As;
                    Tokenizer.ReadNextToken();
                }
                if (alias.Type != AliasType.Implicit || !Tokenizer.IsNextToken(Constants.On))
                {
                    alias.Name = GetIdentifier();
                    join.Alias = alias;
                    if (joinTable != null)
                    {
                        joinTable.Alias = alias;
                    }
                }
                ExpectToken(Constants.On);
                Expression expr = ProcessExpression();

                if (!(expr is CriteriaExpression) &&
                    !(expr is NestedExpression && (expr as NestedExpression).Expression is CriteriaExpression))
                    throw new SyntaxException("Expected Criteria Expression");

                join.Condition = expr;

                table.Joins.Add(join);
            } while (Tokenizer.HasMoreTokens && !Tokenizer.IsNextToken(Constants.Order, Constants.Group));
        }

        protected void ProcessWhere()
        {
            if (Tokenizer.TokenEquals(Constants.Where))
                _statement.Where = ProcessExpression();
        }

        #region Nested type: FieldType

        protected enum FieldType
        {
            Select,
            Update,
            OrderBy,
            GroupBy
        }

        #endregion
    }
}