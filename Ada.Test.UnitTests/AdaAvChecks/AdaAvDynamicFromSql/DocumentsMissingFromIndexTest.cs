namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsMissingFromIndexTest : AdaSingleQueriesTest<DocumentsMissingFromIndex>
    {
        private class DocumentsMissingFromIndexTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    // Docment 23 is missing from docIndex.xml
                    new QueryTestCase(
                        "AVID.SA.17503",
                        new LogEventTestCase
                        {
                            Type = "4.G_7",
                            Tags = new Dictionary<string, string>
                            {
                                {"DocID", @"23"},
                                {"Path", @"AVID.SA.17503.1\Documents\docCollection1\23"}
                            }
                        }
                    ),
                    // No problems
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsMissingFromIndexTestCases)})]
        public void ContextDocBadFileTypesPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}