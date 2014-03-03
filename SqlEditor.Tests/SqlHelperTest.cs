using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.SqlHelpers;

namespace SqlEditor.Tests
{


    /// <summary>
    ///This is a test class for SqlTextExtractorTest and is intended
    ///to contain all SqlTextExtractorTest Unit Tests
    ///</summary>
    [TestClass]
    public class SqlHelperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void GetFirstKeyword_NoComments_Test()
        {
            // Arrange
            const string sql = "SELECT * FROM DUAL";
            const string expected = "SELECT";
            var dbServer = Databases.DatabaseServerFactory.Instance.GetDatabaseServer("DB2");

            // Act
            var actual = SqlHelper.GetFirstKeyword(sql,
                                                   new Regex[] {dbServer.LineCommentRegex, dbServer.BlockCommentRegex});

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void GetFirstKeyword_LineComments_Test()
        {
            // Arrange
            string sql = "-- COMMENT" + Environment.NewLine + "SELECT * FROM DUAL";
            const string expected = "SELECT";
            var dbServer = Databases.DatabaseServerFactory.Instance.GetDatabaseServer("DB2");

            // Act
            var actual = SqlHelper.GetFirstKeyword(sql,
                                                   new Regex[] { dbServer.LineCommentRegex, dbServer.BlockCommentRegex });

            // Assert
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void GetFirstKeyword_BlockComments_Test()
        {
            // Arrange
            string sql = "/* COMMENT " + Environment.NewLine + "*/ " + Environment.NewLine + "SELECT * FROM DUAL";
            const string expected = "SELECT";
            var dbServer = Databases.DatabaseServerFactory.Instance.GetDatabaseServer("DB2");

            // Act
            var actual = SqlHelper.GetFirstKeyword(sql,
                                                   new Regex[] { dbServer.LineCommentRegex, dbServer.BlockCommentRegex });

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
