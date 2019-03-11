namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocBadFileTypesTest : AdaSingleQueriesTest<ContextDocBadFileTypes>
    {
        private class ContextDocBadFileTypesTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17131",
                        new LogEventTestCase
                        {
                            Type = "4.E_9",
                            Tags = new Dictionary<string, string>
                            {
                                {"Path", @"AVID.SA.17131.1\ContextDocumentation\docCollection1\1"},
                                {"FileName", @"1.png"},
                                {"Extension", @".png"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    ),
                    new QueryTestCase(
                        "AVID.SA.17134"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases),
            new object[] {typeof(ContextDocBadFileTypesTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}