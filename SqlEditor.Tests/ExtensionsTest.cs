using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlEditor.SqlHelpers;

namespace SqlEditor.Tests
{


    /// <summary>
    ///This is a test class for SqlTextExtractorTest and is intended
    ///to contain all SqlTextExtractorTest Unit Tests
    ///</summary>
    [TestClass]
    public class ExtensionsTest
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
        [ExpectedException(typeof (OperationCanceledException))]
        public async Task WithCancellation_Test()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            var stopwatch = new Stopwatch();
            const int taskDuration = 10000;

            // Act
            stopwatch.Start();
            var stopTask = new Task(() =>
                                        {
                                            Thread.Sleep(taskDuration/2);
                                            cts.Cancel();
                                        });
            var t = new Task(() =>
                                  {
                                      Console.WriteLine("Task: running ...");
                                      Thread.Sleep(taskDuration);
                                      Console.WriteLine("Task: finished.");
                                  }, TaskCreationOptions.LongRunning);
            stopTask.Start();
            t.Start();
            await t.WithCancellation(cts.Token);

            if (stopwatch.ElapsedMilliseconds >= taskDuration)
            {
                Assert.Fail("Task execution was longer than " + taskDuration);
            }
        }


        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public async Task WithCancellation_Finished_Test()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            const int taskDuration = 1000;

            // Act
            var t = new Task<int>(() =>
            {
                Console.WriteLine("Task: running ...");
                Thread.Sleep(taskDuration);
                Console.WriteLine("Task: finished.");
                return 1;
            }, TaskCreationOptions.LongRunning);
            
            t.Start();
            var result = await t.WithCancellation(cts.Token);

            Assert.AreEqual(1, result);
        }


        /// <summary>
        ///A test for SplitSqlStatements
        ///</summary>
        [TestMethod]
        public void Task_ContinueWith_Test()
        {
            // Arrange
            var num = 0;

            // Act
            var t = new Task(() =>
            {
                Console.WriteLine("Task: running ...");
                Thread.Sleep(3000);
                ++num;
            }, TaskCreationOptions.LongRunning);
            t.Start();

            t.ContinueWith((task => { ++num; }));
            t.Wait();
            
            // Asset
            Assert.AreEqual(2, num);
        }
    }
}
