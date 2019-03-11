namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using Common;
    using Log;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    public abstract class AdaActionBase<TSubject> : AdaActionAtom<TSubject>, IAdaAction<TSubject>
    {
        #region  Constructors

        protected AdaActionBase
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping) : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region Properties

        public Action<string> ProgressCallback { get; set; }

        #endregion

        #region IAdaAction<TSubject> Members

        public ActionRunResult Run(TSubject testSubject)
        {
            try
            {
                if (!CanRun())
                    return ActionRunResult.TestSkippedPreConditionsNotMet;

                if (Skippable(testSubject))
                    return ActionRunResult.FastRun;
                {
                    StartProcessLogging(testSubject);
                    OnRun(testSubject);
                    CompleteProcessLogging();


                    return ActionRunResult.Done;
                }
            }
            catch (Exception e)
            {
                HandleException(e);

                if (e is AdaSkipActionException || e.InnerException is AdaSkipActionException)
                    return ActionRunResult.TestSkipped;
                return ActionRunResult.Error;
            }
        }

        #endregion

        #region

        protected abstract void OnRun(TSubject testSubject);

        #endregion
    }

    public enum ActionRunResult
    {
        Error,
        TestSkipped,
        TestSkippedPreConditionsNotMet,
        FastRun,
        Done
    }

    public abstract class AdaSequence<TSubject> : AdaActionBase<TSubject>
    {
        #region  Fields

        protected readonly List<IAdaAction<TSubject>> Actions = new List<IAdaAction<TSubject>>();

        #endregion

        #region  Constructors

        protected AdaSequence
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping) : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(TSubject testSubject)
        {
            foreach (var action in Actions) action.Run(testSubject);
        }

        #endregion
    }

    public abstract class SimpleTestAction<TSubject> : AdaActionBase<TSubject>
        where TSubject : IAdaRepository, IAdaEntity
    {
        #region  Constructors

        protected SimpleTestAction
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping) : base(processLog, testLog, mapping)
        {
        }

        #endregion
    }

    //public class AdaRepeaterAction<T, V> : AdaActionBase where T : IAdaRepeatableAction<V>
    //{ 

    //    public AdaRepeaterAction(IAdaProcessLog processLog, IAdaTestLog testLog, IEnumerable<V> targets ) : base(processLog, testLog)
    //    {
    //        Activator.CreateInstance<T>();
    //    }
    //}


    //public interface IAdaRepeatableAction<T>:IAdaAction
    //{
    //     void Run(T);
    //}

    //public class AdaActionSequence<T, V> : AdaActionBase where T : IAdaAction<V>
    //{

    //    public AdaActionSequence(IAdaProcessLog processLog, IAdaTestLog testLog, IEnumerable<V> targets) : base(processLog, testLog)
    //    {
    //        Activator.CreateInstance<T>();
    //    }
    //}


    //public class TestAction : AdaRepeaterAction<testrepeater, IAdaProcessLog>
    //{
    //    public TestAction(IAdaProcessLog processLog, IAdaTestLog testLog, IEnumerable<IAdaProcessLog> targets) : base(processLog, testLog, targets)
    //    {

    //    }


    //}

    //public class testrepeater : IAdaRepeatableAction<IAdaProcessLog>
    //{
    //    public testrepeater(IAdaProcessLog processLog, IAdaTestLog testLog) : base(processLog, testLog)
    //    {

    //    }

    //}
}