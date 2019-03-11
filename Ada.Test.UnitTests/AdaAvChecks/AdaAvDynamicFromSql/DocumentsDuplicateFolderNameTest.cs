namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsDuplicateFolderNameTest : AdaSingleQueriesTest<DocumentsDuplicateFolderName>
    {
        private class DocumentsDuplicateFolderNameTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17509",
                        new LogEventTestCase
                        {
                            Type = "4.G_13",
                            Tags = new Dictionary<string, string>
                            {
                                {"DocumentID", @"4"},
                                {"Count", @"2"}
                            }
                        }
                    ),

                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsDuplicateFolderNameTestCases)})]
        public void DocumentsDuplicateFolderNamePositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}