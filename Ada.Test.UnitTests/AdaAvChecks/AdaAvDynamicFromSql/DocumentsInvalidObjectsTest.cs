namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsInvalidObjectsTest : AdaSingleQueriesTest<DocumentsInvalidObjects>
    {
        private class DocumentsInvalidObjectsTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17186",
                        new LogEventTestCase
                        {
                            Type = "4.G_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"1.tif"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.G_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"MappeX"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsInvalidObjectsTestCases)})]
        public void DocumentsInvalidObjectsPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}