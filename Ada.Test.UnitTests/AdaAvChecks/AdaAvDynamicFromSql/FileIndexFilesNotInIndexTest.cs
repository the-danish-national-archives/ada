namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.FileIndex;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class FileIndexFilesNotInIndexTest : AdaSingleQueriesTest<FileIndexFilesNotInIndex>
    {
        private class FileIndexFilesNotInIndexTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17130",
                        new LogEventTestCase
                        {
                            Type = "4.C.2_2",
                            Tags = new Dictionary<string, string>
                            {
                                {
                                    "Path",
                                    @"AVID.SA.17130.1\ContextDocumentation\docCollection1\10\1.tif"
                                }
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(FileIndexFilesNotInIndexTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}