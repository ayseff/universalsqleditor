using System.Collections.Generic;
using SqlEditor.SqlParser.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.SqlParser.Entities;

namespace SqlEditor.SqlParser.Tests
{
    
    
    /// <summary>
    ///This is a test class for SelectStatementParserTest and is intended
    ///to contain all SelectStatementParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SelectStatementParserTest : TestBase
    {
        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod()]
        public void ExecuteTest_No_Table()
        {
            const string sql = "SELECT * FROM";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Tables.Count);
        }

        [TestMethod()]
        public void ParserFactory_ExecuteTest()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            string sql = string.Format("SELECT * FROM {0}", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_No_From()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Single_Table_No_Alias()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Single_Table_Alias()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP" }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES EMP";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Single_Table_Alias_AS()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Single_Table_Schema()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = "HR" };
            const string sql = "SELECT * FROM HR.EMPLOYEES AS EMP";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Single_Table_Alias_AS_Comma()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP,";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Single_Table_Alias_AS_Where()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP WHERE";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Tables_Comma_Separated()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.Implicit }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP, JOBS J";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Tables_Join()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Tables_Inner_Join()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP INNER JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Tables_Left_Outer_Join()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP LEFT OUTER JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Tables_Right_Outer_Join()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP RIGHT OUTER JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Tables_Full_Outer_Join()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP FULL OUTER JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Sub_Select()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "JB" }, Schema = null };
            Table table3= new Table { Name = "DATES", Alias = new Alias(null) { Name = "DT", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP, (SELECT JB.* FROM JOBS JB) WHERE DATE = (SELECT 1 FROM DATES as DT)";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
            Assert.IsTrue(AreTablesEqual(table3, actual.Tables[2]));
        }

        [TestMethod()]
        public void ExecuteTest_Select_After_Where()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP WHERE SELECT 1 FROM";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod()]
        public void ExecuteTest_Select_Quote()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT EMP.ID AS \"EMP FROM\", * FROM EMPLOYEES AS EMP FULL OUTER JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }

        [TestMethod()]
        public void ExecuteTest_Two_Selects_Semicolon()
        {
            Table table1 = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "EMP", Type = AliasType.As }, Schema = null };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "J", Type = AliasType.As }, Schema = null };
            const string sql = "SELECT * FROM EMPLOYEES AS EMP FULL OUTER JOIN JOBS AS J ON EMP.ID = J.ID;SELECT * FROM EMPLOYEES AS EMP FULL OUTER JOIN JOBS AS J ON EMP.ID = J.ID";
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(2, statements.Count);
            IStatement actualStatement = statements[0];
            SelectStatement actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));

            actualStatement = statements[1];
            actual = actualStatement as SelectStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table1, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }
    }
}
