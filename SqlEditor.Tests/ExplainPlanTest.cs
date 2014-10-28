using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.DatabaseExplorer;
using SqlEditor.SqlHelpers;

namespace SqlEditor.Tests
{


    /// <summary>
    ///This is a test class for SqlTextExtractorTest and is intended
    ///to contain all SqlTextExtractorTest Unit Tests
    ///</summary>
    [TestClass]
    public class ExplainPlanTest
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
        public void Db2Explain_Test()
        {
            // Arrange
            const string sql = "SELECT * FROM DUAL";
            const string expected = "SELECT";
            var dbServer = Databases.DatabaseServerFactory.Instance.GetDatabaseServer("DB2");

            var sb = dbServer.GetConnectionStringBuilder(
                "Database=CDWDEVI;User ID=cdwusr;Server=C3DUDB1:50601;Connection Lifetime=600;Connection Reset=True;Max Pool Size=20;Min Pool Size=1;QueryTimeout=0;IsolationLevel=ReadUnCommitted;Password=cdw724e7;");

            var conn = new DatabaseConnection();
            conn.DatabaseServer = dbServer;
            conn.ConnectionString = sb.ConnectionString;
            

            // Act
            var explain = dbServer.GetExplainPlanGenerator()
                .GetExplainPlan(conn,
                    " SELECT * from cdw.ABN_IND i inner join cdw.CLNCL_FNG c on c.clncl_fng_sk = i.clncl_fng_sk where i.clncl_fng_sk = ?");

            // Assert
            Assert.AreEqual(expected, "");
        }

        
    }
}
