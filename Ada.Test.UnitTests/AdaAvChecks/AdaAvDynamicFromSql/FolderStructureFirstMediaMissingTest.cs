namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.FolderStructure;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class FolderStructureFirstMediaMissingTest : AdaSingleQueriesTest<FolderStructureFirstMediaMissing>
    {
        private class FolderStructureFirstMediaMissingTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17284",
                        new LogEventTestCase
                        {
                            Type = "4.B.1_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"MediaNumber", @"1"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(FolderStructureFirstMediaMissingTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}