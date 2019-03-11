namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class SchemaMissingFolderTest : AdaSingleQueriesTest<SchemaMissingFolder>
    {
        private class SchemaMissingFolderTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17270",
                        new LogEventTestCase
                        {
                            Type = "4.F_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"folderName", @"localShared"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.F_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"folderName", @"standard"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(SchemaMissingFolderTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}