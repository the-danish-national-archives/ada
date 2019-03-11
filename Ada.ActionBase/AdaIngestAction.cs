namespace Ada.ActionBase
{
    #region Namespace Using

    using Common;
    using Log;
    using Log.Entities;
    using Repositories;

    #endregion

    public abstract class AdaIngestAction<TEntity, TRepository, TLoader> : AdaActionBase<TLoader> where TRepository : IAdaIngestRepository<TEntity>
    {
        #region  Fields

        protected TRepository TargetRepository;

        #endregion

        #region  Constructors

        protected AdaIngestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, TRepository targetRepository) : base(processLog, testLog, mapping)
        {
            TargetRepository = targetRepository;
        }

        #endregion

        #region

        protected override void Clear(ProcessLogEntry previousProcessLogEntry)
        {
            base.Clear(previousProcessLogEntry);
            TargetRepository.Clear();
        }

        #endregion
    }
}