#region Header

// Author 
// Created 19

#endregion

namespace Ada.Test.UnitTests.AdaAvChecks.PrimaryKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using AdaAvDynamicFromSql;
    using AutoRunTestsuite;
    using Checks.Table;
    using Common;
    using Log;
    using NUnit.Framework;
    using Repositories;

    #endregion

    public class TableMissnamedXmlTest : AgainstPreLoadedDatabaseTestWithAv<TableMissnamedXml>
    {
        #region

        protected override void Act
        (
            AdaProcessLog adaProcessLog,
            AdaTestLog adaTestLog,
            AdaTestUowFactory adaTestUowFactory,
            AdaAvUowFactory adaAvUowFactory,
            AVMapping mapping)
        {
            using (var repo = new AdaStructureRepo(adaTestUowFactory, 0))
            {
                ReportAny(
                    adaTestLog,
                    TableMissnamedXml.Check(repo));
            }
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases),
            new object[] {typeof(LocalTestCases)})]
        public void PositiveTest
        (
            string inputAvid, IEnumerable<LogEntrySimple> input)
        {
            CollectiveTest(inputAvid, input);
        }

        #endregion

        #region Nested type: LocalTestCases

        private class LocalTestCases : TestCaseSource
        {
            #region Properties

            public override string Catagory { get; } = "AgainstPreLoadedDatabaseTestWithAv";

            public override List<QueryTestCase> Tests =>
                new List<QueryTestCase>
                {
                    new QueryTestCase(
                        "AVID.SA.17268",
                        new LogEventTestCase
                        {
                            Type = "4.D_7",
                            Tags = new Dictionary<string, string>
                            {
                                {"Path", @"AVID.SA.17268.1\Tables\table2"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.18000"
                    )
                };

            #endregion
        }

        #endregion
    }
}