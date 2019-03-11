namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocMaxCountTest : AdaSingleQueriesTest<ContextDocMaxCount>
    {
        private class ContextDocMaxCountTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17211",
                        new LogEventTestCase
                        {
                            Type = "4.E_10",
                            Tags = new Dictionary<string, string>
                            {
                                {"FolderName", @"docCollection1"},
                                {"Count", @"10999"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocMaxCountTestCases)})]
        public void ContextDocMaxCountPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}