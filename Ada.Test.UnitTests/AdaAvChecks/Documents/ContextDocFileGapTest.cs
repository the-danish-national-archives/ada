#region Header

// Author 
// Created 19

#endregion

namespace Ada.Test.UnitTests.AdaAvChecks.Documents
{
    #region Namespace Using

    using System.Collections.Generic;
    using AdaAvDynamicFromSql;
    using AutoRunTestsuite;
    using Checks.Documents.ContextDocOnDisk;
    using Common;
    using Log;
    using NUnit.Framework;
    using Repositories;

    #endregion

    public class ContextDocFileGapTest : AgainstPreLoadedDatabaseTestWithAv<ContextDocFileGap>
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
                ReportAny(adaTestLog, ContextDocFileGap.Check(repo));
            }
        }

        [TestCaseSource(typeof(TestExtractor), nameof(TestExtractor.TestCases),
            new object[] {typeof(LocalTestCases)})]
        public void PositiveTest
        (
            string inputAvid,
            IEnumerable<LogEntrySimple> input)
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
                        "AVID.SA.17293",
                        new LogEventTestCase
                        {
                            Type = "4.E_13",
                            Tags = new Dictionary<string, string>
                            {
                                {"Path", @"AVID.SA.17293.1\ContextDocumentation\docCollection1\2"}
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