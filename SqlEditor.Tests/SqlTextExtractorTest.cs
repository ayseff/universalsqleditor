using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.Databases.Db2;
using SqlEditor.SqlHelpers;

namespace SqlEditor.Tests
{


    /// <summary>
    ///This is a test class for SqlTextExtractorTest and is intended
    ///to contain all SqlTextExtractorTest Unit Tests
    ///</summary>
    [TestClass]
    public class SqlTextExtractorTest
    {
        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatementsTest()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "SELECT * FROM TABLE1;\r\nSELECT * FROM TABLE2;";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
            Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_Spaces_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "  SELECT * FROM TABLE1;  \r\n   SELECT * FROM TABLE2;    ";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
            Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
        }


        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_NewLine_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "  SELECT * \nfrom TABLE1;  \r\n   SELECT * FROM TABLE2;    ";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("SELECT *\nfrom TABLE1", actual[0]);
            Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_NoTerminator_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "  SELECT * FROM TABLE1;  \r\n   SELECT * FROM TABLE2";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
            Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_MultiTerminator_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "  SELECT * FROM TABLE1;;  \r\n   SELECT * FROM TABLE2";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
            Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
        }


        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_LiteralTerminator_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "  SELECT ';',* FROM TABLE1;  \r\n   SELECT * FROM TABLE2";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("SELECT ';',* FROM TABLE1", actual[0]);
            Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_LineComment_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "  SELECT ';',* FROM TABLE1;  \r\n--SELECT * FROM TABLE2";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("SELECT ';',* FROM TABLE1", actual[0]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_LineComment2_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "--SELECT * FROM TABLE3\r\n  SELECT ';',* FROM TABLE1;  \r\n--SELECT * FROM TABLE2";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("SELECT ';',* FROM TABLE1", actual[0]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_BlockComment_SingleLine_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "/*SELECT * FROM TABLE3*/\r\n  SELECT ';',* FROM TABLE1;";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("SELECT ';',* FROM TABLE1", actual[0]);
        }

        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void SplitSqlStatements_BlockComment_MultiLine_Test()
        {
            // Arrange
            var target = new SqlTextExtractor(new[] { ";" }, new Db2DatabaseServer());
            const string text = "/*SELECT * FROM TABLE3;\r\nSELECT * FROM TEST;\r\n  */SELECT ';',* FROM TABLE1;";

            // Act
            var actual = target.SplitSqlStatements(text);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("SELECT ';',* FROM TABLE1", actual[0]);
        }
    }

    //    /// <summary>
    //    ///Gets or sets the test context which provides
    //    ///information about and functionality for the current test run.
    //    ///</summary>
    //    public TestContext TestContext { get; set; }

    //    #region Additional test attributes
    //    // 
    //    //You can use the following additional attributes as you write your tests:
    //    //
    //    //Use ClassInitialize to run code before running the first test in the class
    //    //[ClassInitialize()]
    //    //public static void MyClassInitialize(TestContext testContext)
    //    //{
    //    //}
    //    //
    //    //Use ClassCleanup to run code after all tests in a class have run
    //    //[ClassCleanup()]
    //    //public static void MyClassCleanup()
    //    //{
    //    //}
    //    //
    //    //Use TestInitialize to run code before running each test
    //    //[TestInitialize()]
    //    //public void MyTestInitialize()
    //    //{
    //    //}
    //    //
    //    //Use TestCleanup to run code after each test has run
    //    //[TestCleanup()]
    //    //public void MyTestCleanup()
    //    //{
    //    //}
    //    //
    //    #endregion


    //    /// <summary>
    //    ///A test for SplitSqlStatements
    //    ///</summary>
    //    [TestMethod]
    //    public void SplitSqlStatementsTest()
    //    {
    //        // Arrange
    //        var target = new SqlTextExtractor(new[] { ";"});
    //        const string text = "SELECT * FROM TABLE1;\r\nSELECT * FROM TABLE2;";
            
    //        // Act
    //        var actual = target.SplitSqlStatements(text);

    //        // Assert
    //        Assert.AreEqual(2, actual.Count);
    //        Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
    //        Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
    //    }

    //    /// <summary>
    //    ///A test for SplitSqlStatements
    //    ///</summary>
    //    [TestMethod]
    //    public void SplitSqlStatements_Spaces_Test()
    //    {
    //        // Arrange
    //        var target = new SqlTextExtractor(new[] { ";" });
    //        const string text = "  SELECT * FROM TABLE1;  \r\n   SELECT * FROM TABLE2;    ";

    //        // Act
    //        var actual = target.SplitSqlStatements(text);

    //        // Assert
    //        Assert.AreEqual(2, actual.Count);
    //        Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
    //        Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
    //    }
        
        
    //    /// <summary>
    //    ///A test for SplitSqlStatements
    //    ///</summary>
    //    [TestMethod]
    //    public void SplitSqlStatements_NewLine_Test()
    //    {
    //        // Arrange
    //        var target = new SqlTextExtractor(new[] { ";" });
    //        const string text = "  SELECT * \nfrom TABLE1;  \r\n   SELECT * FROM TABLE2;    ";

    //        // Act
    //        var actual = target.SplitSqlStatements(text);

    //        // Assert
    //        Assert.AreEqual(2, actual.Count);
    //        Assert.AreEqual("SELECT *\nfrom TABLE1", actual[0]);
    //        Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
    //    }

    //    /// <summary>
    //    ///A test for SplitSqlStatements
    //    ///</summary>
    //    [TestMethod]
    //    public void SplitSqlStatements_NoTerminator_Test()
    //    {
    //        // Arrange
    //        var target = new SqlTextExtractor(new[] { ";" });
    //        const string text = "  SELECT * FROM TABLE1;  \r\n   SELECT * FROM TABLE2";

    //        // Act
    //        var actual = target.SplitSqlStatements(text);

    //        // Assert
    //        Assert.AreEqual(2, actual.Count);
    //        Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
    //        Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
    //    }

    //    /// <summary>
    //    ///A test for SplitSqlStatements
    //    ///</summary>
    //    [TestMethod]
    //    public void SplitSqlStatements_MultiTerminator_Test()
    //    {
    //        // Arrange
    //        var target = new SqlTextExtractor(new[] { ";" });
    //        const string text = "  SELECT * FROM TABLE1;;  \r\n   SELECT * FROM TABLE2";

    //        // Act
    //        var actual = target.SplitSqlStatements(text);

    //        // Assert
    //        Assert.AreEqual(2, actual.Count);
    //        Assert.AreEqual("SELECT * FROM TABLE1", actual[0]);
    //        Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
    //    }


    //    /// <summary>
    //    ///A test for SplitSqlStatements
    //    ///</summary>
    //    [TestMethod]
    //    public void SplitSqlStatements_LiteralTerminator_Test()
    //    {
    //        // Arrange
    //        var target = new SqlTextExtractor(new[] { ";" });
    //        const string text = "  SELECT ';',* FROM TABLE1;  \r\n   SELECT * FROM TABLE2";

    //        // Act
    //        var actual = target.SplitSqlStatements(text);

    //        // Assert
    //        Assert.AreEqual(2, actual.Count);
    //        Assert.AreEqual("SELECT ';',* FROM TABLE1", actual[0]);
    //        Assert.AreEqual("SELECT * FROM TABLE2", actual[1]);
    //    }
    //}
}
