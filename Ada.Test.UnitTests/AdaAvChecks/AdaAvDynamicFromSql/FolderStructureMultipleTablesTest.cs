namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.FolderStructure;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class FolderStructureMultipleTablesTest : AdaSingleQueriesTest<FolderStructureMultipleTables>
    {
        private class FolderStructureMultipleTablesTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17124",
                        new LogEventTestCase
                        {
                            Type = "4.B.2_9",
                            Tags = new Dictionary<string, string>
                            {
                                {"MedieNumber", @"1"}
                            }
                        }
                        , new LogEventTestCase
                        {
                            Type = "4.B.2_9",
                            Tags = new Dictionary<string, string>
                            {
                                {"MedieNumber", @"2"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000",
                        new LogEventTestCase
                        {
                            Type = "4.B.2_9",
                            Tags = new Dictionary<string, string>
                            {
                                {"MedieNumber", @"1"}
                            }
                        }
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(FolderStructureMultipleTablesTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}