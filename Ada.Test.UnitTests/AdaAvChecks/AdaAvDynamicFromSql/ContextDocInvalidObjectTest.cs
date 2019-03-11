namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ContextDocInvalidObjectTest : AdaSingleQueriesTest<ContextDocInvalidObject>
    {
        private class ContextDocInvalidObjectTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17246",
                        new LogEventTestCase
                        {
                            Type = "4.E_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"1.tif"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.E_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"2.tif"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.E_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"3.tif"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.E_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"4.tif"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.E_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"5.tif"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.E_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ObjectName", @"docColection2"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(ContextDocInvalidObjectTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}