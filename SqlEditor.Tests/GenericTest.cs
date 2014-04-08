using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;
using SqlEditor.SqlHelpers;
using Utilities.IO.Sync;

namespace SqlEditor.Tests
{


    /// <summary>
    ///This is a test class for SqlTextExtractorTest and is intended
    ///to contain all SqlTextExtractorTest Unit Tests
    ///</summary>
    [TestClass]
    public class GenericTest
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
        public void GenerateOracleViews_Test()
        {
            var sb = new StringBuilder();
            var cnStringBuilder = new OracleConnectionStringBuilder();
            cnStringBuilder.DataSource =
                "(DESCRIPTION =     (ADDRESS = (PROTOCOL = TCP)(HOST = XPS15)(PORT = 1521))    (CONNECT_DATA =      (SERVER = DEDICATED)      (SERVICE_NAME = XE)    )  )";
            cnStringBuilder.UserID = "system";
            cnStringBuilder.Password = "a09182338A!";
            var data = new List<NameValuePair<string, string>>();
            using (var connection = new OracleConnection(cnStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "select distinct creator, tname from syscatalog where tname like 'ALL%' OR tname like 'USER%' and tname = 'DBA%' order by creator, tname";
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var schema = dr.GetString(0);
                            var tableName = dr.GetString(1);
                            var vp = new NameValuePair<string, string>(schema, tableName);
                            data.Add(vp);
                        }
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    foreach (var nameValuePair in data)
                    {
                        command.CommandText =
                            "select * from " + nameValuePair.Name + "." + nameValuePair.Value;
                        using (var dr = command.ExecuteReader(CommandBehavior.SchemaOnly))
                        {
                            var schemaTable = dr.GetSchemaTable();
                            if (schemaTable != null)
                            {
                                var columnNames = schemaTable.Rows.Cast<DataRow>().Select(x => x[0].ToString());
                                foreach (var column in columnNames)
                                {
                                    sb.Append("new[] { ");
                                    sb.Append("\"");
                                    sb.Append(nameValuePair.Name);
                                    sb.Append("\", \"");
                                    sb.Append(nameValuePair.Value);
                                    sb.Append("\", \"");
                                    sb.Append(column);
                                    sb.AppendLine("\" },");
                                }
                            }
                        }
                    }
                }
                File.WriteAllText(@"C:\Users\Mensur\oracle.csv", sb.ToString());
            }
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
