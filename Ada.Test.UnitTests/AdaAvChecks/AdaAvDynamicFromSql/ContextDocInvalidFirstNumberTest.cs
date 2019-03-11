namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocInvalidFirstNumberTest : AdaSingleQueriesTest<ContextDocInvalidFirstNumber>
    {
        private class ContextDocInvalidFirstNumberTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    // With no errors
                    new QueryTestCase(
                        "AVID.SA.17244"
                    ),
                    new QueryTestCase(
                        "AVID.SA.17245",
                        new LogEventTestCase
                        {
                            Type = "4.E_4",
                            Tags = new Dictionary<string, string>
                            {
                                {"Gaps", @"1"},
                                {"Folder", @"ContextDocumentation"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.17243",
                        new LogEventTestCase
                        {
                            Type = "4.E_4",
                            Tags = new Dictionary<string, string>
                            {
                                {"Gaps", @"1"},
                                {"Folder", @"ContextDocumentation"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocInvalidFirstNumberTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}