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

    public class PrimaryKeyNullTest : AgainstPreLoadedDatabaseTestWithAv<PrimaryKeyNull>
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
                            PrimaryKeyNull.Check(
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
                        "AVID.SA.17255" // Invalid due to datatype
                    ),
                    new QueryTestCase(
                        "AVID.SA.17256",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"ConstraintName", @"PK_DOKTABEL"},
                                {"TableName", @"DOKTABEL"},
                                {"Count", @"10"},
                                {"TotalRows", @"23"},
                                {"ColumnNames", @"DokumentID"},
                                {"Percent", @"43.48"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.17257",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"ConstraintName", @"PK_AGG"},
                                {"TableName", @"AGG"},
                                {"Count", @"1"},
                                {"TotalRows", @"4"},
                                {"ColumnNames", @"AmtID"},
                                {"Percent", @"25.00"}
                            }
                        }
                    ),
                    new QueryTestCase(
                        "AVID.SA.17258" // invalid due to schema
                    ),
                    new QueryTestCase(
                        "AVID.SA.17259",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_2",
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
                        "AVID.SA.17260" // invalid due to schema
                    ),
                    new QueryTestCase(
                        "AVID.SA.17261",
                        new LogEventTestCase
                        {
                            Type = "4.A_1_2",
                            Tags = new Dictionary<string, string>
                            {
                                {"ConstraintName", @"PK_AGG"},
                                {"TableName", @"AGG"},
                                {"Count", @"15"},
                                {"TotalRows", @"22710"},
                                {"ColumnNames", @"AmtID,ArtID,Aar"},
                                {"Percent", @"0.07"}
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