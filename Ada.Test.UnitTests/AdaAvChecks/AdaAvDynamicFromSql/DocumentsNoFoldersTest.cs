namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsNoFoldersTest : AdaSingleQueriesTest<DocumentsNoFolders>
    {
        private class DocumentsNoFoldersTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17505",
                        new LogEventTestCase
                        {
                            Type = "4.G_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"FolderName", @"\Documents"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.17252"
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsNoFoldersTestCases)})]
        public void DocumentsNoFoldersPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}