#region Header

// Author 
// Created 19

#endregion

namespace Ada.Test.UnitTests.AdaAvChecks
{
    #region Namespace Using

    using Common;
    using Log;
    using Repositories;

    #endregion

    public abstract class AgainstPreLoadedDatabaseTestWithAv<T> : AgainstPreLoadedDatabaseTest<T>
    {
        #region

        protected sealed override void Act
            (AdaProcessLog adaProcessLog, AdaTestLog adaTestLog, AdaTestUowFactory adaTestUowFactory, AVMapping mapping)
        {
            var adaAvUowFactory = new AdaAvUowFactory(Id, "av", DatabasePath);

            Act(adaProcessLog, adaTestLog, adaTestUowFactory, adaAvUowFactory, mapping);
        }

        protected abstract void Act
        (
            AdaProcessLog adaProcessLog,
            AdaTestLog adaTestLog,
            AdaTestUowFactory adaTestUowFactory,
            AdaAvUowFactory adaAvUowFactory,
            AVMapping mapping);

        #endregion
    }
}