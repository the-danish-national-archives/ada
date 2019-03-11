#region Header

// Author 
// Created 19

#endregion

namespace Ada.Test.UnitTests.AdaAvChecks.ForeignKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using AdaAvDynamicFromSql;
    using AutoRunTestsuite;
    using Checks.ForeignKey;
    using Common;
    using Log;
    using NUnit.Framework;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class ForeignKeyWithNoTargetTest : AgainstPreLoadedDatabaseTestWithAv<ForeignKeyWithNoTarget>
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
            using (var testuow = adaTestUowFactory.GetUnitOfWork())
            {
                var fks = testuow.GetRepository<ForeignKey>().All().ToList();

                foreach (var fk in fks)
                    using (var sqlRepo = new SimpleAvSqlRepo(adaAvUowFactory))
                    {
                        ReportAny(
                            adaTestLog,
                            ForeignKeyWithNoTarget.Check(
                                fk,
                                sqlRepo));
                    }
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
                        "AVID.SA.17264",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_5",
                            Tags = new Dictionary<string, string>
                            {
                                {"ConstraintName", @"FK_IndeksTerm_M2MTABEL"},
                                {"TableName", @"M2MTABEL"},
                                {"Count", @"2"},
                                {"TotalRows", @"5"},
                                {"ColumnNames", @"IndekstermID"},
                                {"Percent", @"40.00"}
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