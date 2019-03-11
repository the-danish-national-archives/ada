namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using AutoRunTestsuite;
    using Checks.FileIndex;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class FileIndexBadCheckSumTest : AdaSingleQueriesTest<FileIndexBadCheckSum>
    {
        private class FileIndexBadCheckSumTestCases : TestCaseSourceForAdaSingleQuery
        {
            #region Properties

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17062",
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {
                                    "Path",
                                    @"AVID.SA.17062.1\ContextDocumentation\docCollection1\1\1.tif"
                                },
                                {"MD5", @"72a7c634c3b3b3e1d2d7fd953209b080"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17062.1\Indices\archiveIndex.xml"},
                                {"MD5", @"787EB5CC65EA5CFD07C9A35F21F9705E"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17062.1\Tables\table1\table1.xsd"},
                                {"MD5", @"EE1996C5C126EDC30B63535CD202501F"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17062.1\Tables\table2\table2.xsd"},
                                {"MD5", @"CBDC27A90B4D24246E0AB7B287BBC722"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17062.1\Tables\table3\table3.xsd"},
                                {"MD5", @"9F93A50EDD6C35A0B44447622BDFE66E"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17062.1\Tables\table4\table4.xsd"},
                                {"MD5", @"7D5A4355B7FFBEDEBFBF6F31DD0946C1"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.17062.1\Tables\table5\table5.xsd"},
                                {"MD5", @"0CD61F8C0AAB6C03400E8AEBA849FFEF"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000",
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.18000.1\Tables\table1\table1.xsd"},
                                {"MD5", @"322D2CE8C2B9E878867F0A23BC5D89B0"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.18000.1\Tables\table2\table2.xsd"},
                                {"MD5", @"CBDC27A90B4D24246E0AB7B287BBC722"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.18000.1\Tables\table3\table3.xsd"},
                                {"MD5", @"9F93A50EDD6C35A0B44447622BDFE66E"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.18000.1\Tables\table4\table4.xsd"},
                                {"MD5", @"7D5A4355B7FFBEDEBFBF6F31DD0946C1"}
                            }
                        },
                        new LogEventTestCase
                        {
                            Type = "4.C.2_3",
                            Tags = new Dictionary<string, string>
                            {
                                {"mediaNumber", @"1"},
                                {"Path", @"AVID.SA.18000.1\Tables\table5\table5.xsd"},
                                {"MD5", @"0CD61F8C0AAB6C03400E8AEBA849FFEF"}
                            }
                        }
                    )
                };

            #endregion
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases),
            new object[] {typeof(FileIndexBadCheckSumTestCases)})]
        public void PositiveTest
        (
            string inputAvid,
            IEnumerable<LogEntrySimple> inputXml)
        {
            CollectiveTest(inputAvid, inputXml);
        }
    }
}