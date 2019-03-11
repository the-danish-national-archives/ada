namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocDuplicateFolderNameTest : AdaSingleQueriesTest<ContextDocDuplicateFolderName>
    {
        private class ContextDocDuplicateFolderNameTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    // Expect none, but it is written in in the testsuite descriptions
                    new QueryTestCase(
                        "AVID.SA.17105"
                    ),
                    new QueryTestCase(
                        "AVID.SA.17251",
                        new LogEventTestCase
                        {
                            Type = "4.E_12",
                            Tags = new Dictionary<string, string>
                            {
                                {"DocumentID", @"3"},
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

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocDuplicateFolderNameTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}