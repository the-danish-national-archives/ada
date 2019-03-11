namespace Ada.Repositories
{
    #region Namespace Using

    using System.Linq;
    using Ra.DomainEntities;

    #endregion

    public abstract class IngestEntityRepoBase<TEntity> : EntityRepoBase<TEntity>, IAdaIngestRepository<TEntity> where TEntity : class, IAdaEntity
    {
        #region  Constructors

        protected IngestEntityRepoBase(IAdaUowFactory factory) : base(factory)
        {
        }

        #endregion

        #region IAdaIngestRepository<TEntity> Members

        public void SaveEntity(TEntity entity)
        {
            using (var uow = Factory.GetUnitOfWork())
            {
                uow.BeginTransaction();
                var repo = uow.GetRepository<TEntity>();
                repo.Add(entity);
                uow.Commit();
            }
        }

        public virtual void Clear()
        {
            Factory.CleanUpTable<TEntity>();
        }

        #endregion
    }


    public abstract class EntityRepoBase<TEntity> : IAdaEntityRepository<TEntity>
        where TEntity : class, IAdaEntity
    {
        #region  Fields

        protected readonly IAdaUowFactory Factory;

        #endregion

        #region  Constructors

        protected EntityRepoBase(IAdaUowFactory factory)
        {
            Factory = factory;
        }

        #endregion

        #region IAdaEntityRepository<TEntity> Members

        public TEntity LoadEntity()
        {
            using (var uow = Factory.GetUnitOfWork())
            {
                var repo = uow.GetRepository<TEntity>();
                return repo.All().FirstOrDefault();
            }
        }

        #endregion
    }
}