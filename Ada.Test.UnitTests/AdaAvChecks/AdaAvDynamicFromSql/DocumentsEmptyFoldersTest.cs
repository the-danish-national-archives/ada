namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsEmptyFoldersTest : AdaSingleQueriesTest<DocumentsEmptyFolders>
    {
        private class DocumentsEmptyFoldersTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17187",
                        new LogEventTestCase
                        {
                            Type = "4.G_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"FolderName", @"docCollection2"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsEmptyFoldersTestCases)})]
        public void DocumentsEmptyFoldersPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}