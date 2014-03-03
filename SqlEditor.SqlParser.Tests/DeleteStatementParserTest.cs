#region

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Tests
{
    [TestClass]
    public class DeleteStatementParserTest : TestBase
    {
        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod]
        public void ExecuteTest_No_Table()
        {
            string sql = string.Format("DELETE");
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Tables.Count);
        }

        [TestMethod]
        public void ExecuteTest_Into_No_Table()
        {
            string sql = string.Format("DELETE FROM ");
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Tables.Count);
        }

        [TestMethod]
        public void ExecuteTest_Column_List_Values()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            string sql = string.Format("DELETE FROM {0}", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_No_Column_List_Values()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            string sql = string.Format("DELETE FROM {0} WHERE ID = 2", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Alias_AS()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "BR", Type = AliasType.As }, Schema = null };
            string sql = string.Format("DELETE FROM {0} AS BR WHERE BR.ID = 2", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Alias()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = "BR" }, Schema = null };
            string sql = string.Format("DELETE FROM {0} BR WHERE BR.ID = 2", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Schema()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = "HR" };
            string sql = string.Format("DELETE FROM {0}.{1} WHERE", table.Schema, table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Select_Schema()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = "HR" };
            string sql = string.Format("DELETE FROM {0}.{1}  WHERE ID = (SELECT * FROM {0}.{1})", table.Schema, table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[1]));
        }

        [TestMethod]
        public void ExecuteTest_Select_Schema_Alias()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = "HR" };
            Table table2 = new Table { Name = "JOBS", Alias = new Alias(null) { Name = "JB", Type = AliasType.As }, Schema = "HR" };
            string sql = string.Format("DELETE FROM {0}.{1} WHERE ID = (SELECT id1, id2 FROM {2}.{3} AS {4})", table.Schema, table.Name, table2.Schema, table2.Name, table2.Alias.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            DeleteStatement actual = actualStatement as DeleteStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }
    }
}
