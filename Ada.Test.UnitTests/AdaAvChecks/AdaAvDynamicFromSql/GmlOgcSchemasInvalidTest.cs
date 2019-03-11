namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Gml;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class GmlOgcSchemasInvalidTest : AdaSingleQueriesTest<GmlOgcSchemasInvalid>
    {
        private class GmlOgcSchemasInvalidTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17297",
                        new LogEventTestCase
                        {
                            Type = "5.G_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"Name", @"dataQuality.xsd"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(GmlOgcSchemasInvalidTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }
    }
}