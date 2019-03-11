namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsBadFileTypesTest : AdaSingleQueriesTest<DocumentsBadFileTypes>
    {
        private class DocumentsBadFileTypesTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17508",
                        new LogEventTestCase
                        {
                            Type = "4.G_9",
                            Tags = new Dictionary<string, string>
                            {
                                {"Path", @"AVID.SA.17508.1\Documents\docCollection1\1"},
                                {"FileName", @"1.txt"},
                                {"Extension", @".txt"}
                            }
                        }
                    ),
                    // Should be case insentive
                    new QueryTestCase(
                        "AVID.SA.17300"),
                    new QueryTestCase(
                        "AVID.SA.17341",
                        new LogEventTestCase
                        {
                            Type = "4.G_9",
                            Tags = new Dictionary<string, string>
                            {
                                {"Path", @"AVID.SA.17341.1\Documents\docCollection1\2"},
                                {"FileName", @"1.mp2"},
                                {"Extension", @".mp2"}
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
            new object[] {typeof(DocumentsBadFileTypesTestCases)})]
        public void ContextDocBadFileTypesPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}