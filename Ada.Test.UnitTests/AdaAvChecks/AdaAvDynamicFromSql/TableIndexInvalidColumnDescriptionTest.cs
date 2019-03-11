namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.TableIndex;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class TableIndexInvalidColumnDescriptionTest : AdaSingleQueriesTest<TableIndexInvalidColumnDescription>
    {
        private class TableIndexInvalidColumnDescriptionTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17128",
                        new LogEventTestCase
                        {
                            Type = "6.C_5",
                            Tags = new Dictionary<string, string>
                            {
                                {"ColumnName", @"DokumentID"},
                                {"ColumnNumber", @"c1"},
                                {"ColumnDescription", @"E"},
                                {"TableName", @"DOKTABEL"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "6.C_5",
                            Tags = new Dictionary<string, string>
                            {
                                {"ColumnName", @"Dato"},
                                {"ColumnNumber", @"c2"},
                                {"ColumnDescription", @"O"},
                                {"TableName", @"DOKTABEL"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases),
            new object[] {typeof(TableIndexInvalidColumnDescriptionTestCases)})]
        public void PositiveTest
        (
            string inputAvid,
            IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}