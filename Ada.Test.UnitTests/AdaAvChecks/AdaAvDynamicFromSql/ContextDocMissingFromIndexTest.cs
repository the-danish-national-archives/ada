namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocMissingFromIndexTest : AdaSingleQueriesTest<ContextDocMissingFromIndex>
    {
        private class ContextDocMissingFromIndexTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    // Docment 10 is missing from contextDocumentationIndex.xml
                    new QueryTestCase(
                        "AVID.SA.17093",
                        new LogEventTestCase
                        {
                            Type = "4.E_7",
                            Tags = new Dictionary<string, string>
                            {
                                {"DocID", @"10"},
                                {"Path", @"AVID.SA.17093.1\ContextDocumentation\docCollection1\10"}
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

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocMissingFromIndexTestCases)})]
        public void ContextDocBadFileTypesPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}