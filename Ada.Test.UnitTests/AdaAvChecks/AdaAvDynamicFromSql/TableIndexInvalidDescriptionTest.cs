namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.TableIndex;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class TableIndexInvalidDescriptionTest : AdaSingleQueriesTest<TableIndexInvalidDescription>
    {
        private class TableIndexInvalidDescriptionTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17047",
                        new LogEventTestCase
                        {
                            Type = "6.C_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"TableName", @"INDKSTRM"},
                                {"FolderName", @"table2"},
                                {"TableDescription", @"NA"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(TableIndexInvalidDescriptionTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}