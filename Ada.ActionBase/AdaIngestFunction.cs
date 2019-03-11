namespace Ada.ActionBase
{
    #region Namespace Using

    using Common;
    using Log;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    public abstract class AdaIngestFunction<TSubject, TEntity> : AdaFunctionBase<TSubject, TEntity> where TEntity : class, IAdaEntity
    {
        #region  Constructors

        protected AdaIngestFunction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaEntityRepository<TEntity> targetRepository) : base(processLog, testLog, mapping, targetRepository)
        {
        }

        #endregion
    }
}