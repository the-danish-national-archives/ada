namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class IndicesTableIndexTest : AdaSingleQueriesTest<IndicesTableIndex>
    {
        private class IndicesTableIndexTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17154",
                        new LogEventTestCase
                        {
                            Type = "4.C_4",
                            Tags = new Dictionary<string, string>
                            {
                                {"'tableIndex.xml'", @"tableIndex.xml"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(IndicesTableIndexTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}