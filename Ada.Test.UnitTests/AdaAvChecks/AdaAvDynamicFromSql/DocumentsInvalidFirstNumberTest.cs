namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.Documents.DocumentsOnDisk;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class DocumentsInvalidFirstNumberTest : AdaSingleQueriesTest<DocumentsInvalidFirstNumber>
    {
        private class DocumentsInvalidFirstNumberCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests
                =>
                    new List<QueryTestCase>
                    {
                        new QueryTestCase("AVID.SA.17296",
                            new LogEventTestCase
                            {
                                Type = "4.G_4",
                                Tags = new Dictionary<string, string>
                                {
                                    {
                                        "Gaps",
                                        @"1"
                                    },
                                    {
                                        "Folder",
                                        @"Documents"
                                    }
                                }
                            }),
                        new QueryTestCase("AVID.SA.17500",
                            new LogEventTestCase
                            {
                                Type = "4.G_4",
                                Tags = new Dictionary<string, string>
                                {
                                    {
                                        "Gaps",
                                        @"2"
                                    },
                                    {
                                        "Folder",
                                        @"Documents"
                                    }
                                }
                            }),
                        new QueryTestCase("AVID.SA.17506",
                            new LogEventTestCase
                            {
                                Type = "4.G_4",
                                Tags = new Dictionary<string, string>
                                {
                                    {
                                        "Gaps",
                                        @"1"
                                    },
                                    {
                                        "Folder",
                                        @"Documents"
                                    }
                                }
                            }),
                        new QueryTestCase("AVID.SA.18000"
//                                , LogEventTestCase.Empty
                        )
                    };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases), new object[] {typeof(DocumentsInvalidFirstNumberCases)})]
        public void InvalidFirstNumberPositiveTest(string inputAvid, IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}