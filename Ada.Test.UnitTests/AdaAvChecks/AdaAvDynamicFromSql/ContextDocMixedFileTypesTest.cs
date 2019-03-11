namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocMixedFileTypesTest : AdaSingleQueriesTest<ContextDocMixedFileTypes>
    {
        private class ContextDocMixedFileTypesTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17294",
                        new LogEventTestCase
                        {
                            Type = "4.E_11",
                            Tags = new Dictionary<string, string>
                            {
                                {"Path", @"AVID.SA.17294.1\ContextDocumentation\docCollection1\2"},
                                {"COUNT(DISTINCT extension)", @"2"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocMixedFileTypesTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}