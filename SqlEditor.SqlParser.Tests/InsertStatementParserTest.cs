#region

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.SqlParser.Entities;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Tests
{
    [TestClass]
    public class InsertStatementParserTest : TestBase
    {
        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod]
        public void ExecuteTest_No_Table()
        {
            string sql = string.Format("INSERT ");
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Tables.Count);
        }

        [TestMethod]
        public void ExecuteTest_Into_No_Table()
        {
            string sql = string.Format("INSERT INTO");
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Tables.Count);
        }

        [TestMethod]
        public void ExecuteTest_Column_List_Values()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            string sql = string.Format("INSERT INTO {0} (id, col2) VALUES ('1', '2')", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_No_Column_List_Values()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            string sql = string.Format("INSERT INTO {0} VALUES ('1', '2')", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Alias_AS()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = null };
            string sql = string.Format("INSERT INTO {0} AS BR (id, col2) VALUES ('1', '2')", table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Schema()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = "HR" };
            string sql = string.Format("INSERT INTO {0}.{1} (id, col2) VALUES ('1', '2')", table.Schema, table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
        }

        [TestMethod]
        public void ExecuteTest_Select_Schema()
        {
            Table table = new Table { Name = "EMPLOYEES", Alias = new Alias(null) { Name = null }, Schema = "HR" };
            string sql = string.Format("INSERT INTO {0}.{1} (id, col2) SELECT * FROM {0}.{1}", table.Schema, table.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
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
            string sql = string.Format("INSERT INTO {0}.{1} (id, col2) SELECT id1, id2 FROM {2}.{3} AS {4}", table.Schema, table.Name, table2.Schema, table2.Name, table2.Alias.Name);
            IList<IStatement> statements = ParserFactory.Execute(sql);
            Assert.AreEqual(1, statements.Count);
            IStatement actualStatement = statements[0];
            InsertStatement actual = actualStatement as InsertStatement;
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Tables.Count);
            Assert.IsTrue(AreTablesEqual(table, actual.Tables[0]));
            Assert.IsTrue(AreTablesEqual(table2, actual.Tables[1]));
        }
    }
}
