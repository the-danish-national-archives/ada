namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class SchemaFoldersUnwantedContentsTest : AdaSingleQueriesTest<SchemaFoldersUnwantedContents>
    {
        private class SchemaFoldersUnwantedContentsTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17272",
                        new LogEventTestCase
                        {
                            Type = "4.F_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"1.tif"},
                                {"Folder", @"localShared"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.F_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"1.tif"},
                                {"Folder", @"standard"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.F_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"Thumbs.db"},
                                {"Folder", @"localShared"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.F_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"Thumbs.db"},
                                {"Folder", @"standard"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.F_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"1.tif"},
                                {"Folder", @"schemas"}
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
            new object[] {typeof(SchemaFoldersUnwantedContentsTestCases)})]
        public void PositiveTest
        (
            string inputAvid,
            IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}