namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class IndicesArchiveIndexTest : AdaSingleQueriesTest<IndicesArchiveIndex>
    {
        private class IndicesArchiveIndexTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17152",
                        new LogEventTestCase
                        {
                            Type = "4.C_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"'archiveIndex.xml'", @"archiveIndex.xml"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(IndicesArchiveIndexTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}