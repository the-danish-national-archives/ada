namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.FolderStructure;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class FolderStructureSchemasMissingTest : AdaSingleQueriesTest<FolderStructureSchemasMissing>
    {
        private class FolderStructureSchemasMissingTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17182",
                        new LogEventTestCase
                        {
                            Type = "4.B.2_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"folderName", @"Schemas"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(FolderStructureSchemasMissingTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}