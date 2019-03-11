namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using Ada.Actions;
    using ChecksBase;
    using Common;
    using Log;
    using Repositories;

    #endregion

    public class AdaSingleQueriesTest<T> : AgainstPreLoadedDatabaseTest<T>
        where T : AdaAvDynamicFromSql
    {
        #region

        protected override void Act(AdaProcessLog adaProcessLog, AdaTestLog adaTestLog, AdaTestUowFactory adaTestUowFactory, AVMapping mapping)
        {
            new AdaSingleQueryAction(adaProcessLog, adaTestLog, adaTestUowFactory, mapping).Run(typeof(T));
        }

        #endregion

        #region Nested type: TestCaseSourceForAdaSingleQuery

        public abstract class TestCaseSourceForAdaSingleQuery : TestCaseSource
        {
            #region Properties

            public override string Catagory { get; } = "AdaSingleQuery";

            #endregion
        }

        #endregion
    }
}