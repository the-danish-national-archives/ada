namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocMissingDocTest : AdaSingleQueriesTest<ContextDocMissingDoc>
    {
        private class ContextDocMissingDocTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    // Whole docment folder missing
                    new QueryTestCase(
                        "AVID.SA.17091",
                        new LogEventTestCase
                        {
                            Type = "4.E_6",
                            Tags = new Dictionary<string, string>
                            {
                                {"DocID", @"8"},
                                {"Path", @"\ContextDocumentation\docCollection1\8"}
                            }
                        }
                    ),
                    // Only a file missing
                    new QueryTestCase(
                        "AVID.SA.17092",
                        new LogEventTestCase
                        {
                            Type = "4.E_6",
                            Tags = new Dictionary<string, string>
                            {
                                {"DocID", @"8"},
                                {"Path", @"\ContextDocumentation\docCollection1\8"}
                            }
                        }
                    ),
                    // No problems
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocMissingDocTestCases)})]
        public void ContextDocBadFileTypesPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}