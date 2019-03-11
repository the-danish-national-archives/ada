namespace Ada.Repositories
{
    #region Namespace Using

    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexRepo : IngestEntityRepoBase<TableIndex>
    {
        #region  Constructors

        public TableIndexRepo(IAdaUowFactory factory)
            : base(factory)
        {
        }

        #endregion

        #region

        public override void Clear()
        {
            using (var uow = Factory.GetUnitOfWork())
            {
                uow.BeginTransaction();
                var repo = uow.GetRepository<TableIndex>();
                repo.Delete(repo.All());
                uow.Commit();
            }
        }

        #endregion
    }
}