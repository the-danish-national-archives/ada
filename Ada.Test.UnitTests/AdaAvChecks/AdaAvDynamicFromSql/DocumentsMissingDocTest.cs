namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsMissingDocTest : AdaSingleQueriesTest<DocumentsMissingDoc>
    {
        private class DocumentsMissingDocTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    // Whole docment folder missing
                    new QueryTestCase(
                        "AVID.SA.17502",
                        new LogEventTestCase
                        {
                            Type = "4.G_6",
                            Tags = new Dictionary<string, string>
                            {
                                {"Count", @"1"}
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

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsMissingDocTestCases)})]
        public void ContextDocBadFileTypesPositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}