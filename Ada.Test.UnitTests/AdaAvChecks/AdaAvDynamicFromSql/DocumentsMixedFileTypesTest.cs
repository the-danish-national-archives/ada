namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsMixedFileTypesTest : AdaSingleQueriesTest<DocumentsMixedFileTypes>
    {
        private class DocumentsMixedFileTypesTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests
                =>
                    new List<QueryTestCase>
                    {
                        new QueryTestCase(
                            "AVID.SA.17298",
                            new LogEventTestCase
                            {
                                Type = "4.G_12",
                                Tags = new Dictionary<string, string>
                                {
                                    {"dID", @"2"},
                                    {"Path", @"AVID.SA.17298.1\Documents\docCollection1\2"},
                                    {"Count", @"2"}
                                }
                            }
                        ),
                        new QueryTestCase("AVID.SA.17050"),
                        new QueryTestCase("AVID.SA.18000")
                    };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases),
            new object[] {typeof(DocumentsMixedFileTypesTestCases)})]
        public void PositiveTest(string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}