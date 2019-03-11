#region Header

// Author 
// Created 19

#endregion

namespace Ada.Test.UnitTests.AdaAvChecks.PrimaryKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using AdaAvDynamicFromSql;
    using AutoRunTestsuite;
    using Checks.PrimaryKey;
    using Common;
    using Log;
    using NUnit.Framework;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class PrimaryKeyDuplikatesTest : AgainstPreLoadedDatabaseTestWithAv<PrimaryKeyDuplikates>
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
                var pks = testuow.GetRepository<PrimaryKey>().All().ToList();

                foreach (var pk in pks)
                    using (var sqlRepo = new SimpleAvSqlRepo(adaAvUowFactory))
                    {
                        ReportAny(
                            adaTestLog,
                            PrimaryKeyDuplikates.Check(
                                pk,
                                sqlRepo));
                    }
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
                        "AVID.SA.17252",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ConstraintName", @"PK_AGG"},
                                {"TableName", @"AGG"},
                                {"Count", @"22663"},
                                {"TotalRows", @"22710"},
                                {"ColumnNames", @"Aar"},
                                {"Percent", @"99.79"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.17253",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_1",
                            Tags = new Dictionary<string, string>
                            {
                                {"ConstraintName", @"PK_AGG"},
                                {"TableName", @"AGG"},
                                {"Count", @"1"},
                                {"TotalRows", @"22710"},
                                {"ColumnNames", @"AmtID,ArtID,Aar"},
                                {"Percent", @"0.00"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.17254"
                    )
                };

            #endregion
        }

        #endregion
    }
}