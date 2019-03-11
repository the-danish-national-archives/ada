namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.FileIndex;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class FileIndexFilesMissingTest : AdaSingleQueriesTest<FileIndexFilesMissing>
    {
        private class FileIndexFilesMissingTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17061",
                        new LogEventTestCase
                        {
                            Type = "4.C.2_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17061.1\Documents\docCollection1\16\1.tif"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(FileIndexFilesMissingTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}