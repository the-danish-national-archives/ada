namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using Common;
    using Log;
    using Repositories;

    #endregion

    public abstract class AdaFunctionBase<TInput, TOutput> : AdaActionAtom<TInput> where TOutput : class
    {
        #region  Constructors

        protected AdaFunctionBase(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaRepository targetRepository) : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected abstract TOutput OnRun(TInput testSubject);

        public TOutput Run(TInput testSubject)
        {
            TOutput returnValue = null;
            if (!Skippable(testSubject))
            {
                StartProcessLogging(testSubject);

                try
                {
                    returnValue = OnRun(testSubject);
                }
                catch (Exception e)
                {
                    HandleException(e);
                }

                CompleteProcessLogging();
            }

            return returnValue;
        }

        #endregion
    }
}