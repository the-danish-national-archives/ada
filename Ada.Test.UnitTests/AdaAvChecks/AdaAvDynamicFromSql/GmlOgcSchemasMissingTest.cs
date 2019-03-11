namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Gml;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class GmlOgcSchemasMissingTest : AdaSingleQueriesTest<GmlOgcSchemasMissing>
    {
        private class GmlOgcSchemasMissingTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17040",
                        new LogEventTestCase
                        {
                            Type = "5.G_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"datums.xsd"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "5.G_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"defaultStyle.xsd"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18002"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(GmlOgcSchemasMissingTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}