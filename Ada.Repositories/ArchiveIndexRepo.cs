namespace Ada.Repositories
{
    #region Namespace Using

    using Ra.DomainEntities.ArchiveIndex;

    #endregion

    public class ArchiveIndexRepo : IngestEntityRepoBase<ArchiveIndex>
    {
        #region  Constructors

        public ArchiveIndexRepo(IAdaUowFactory factory)
            : base(factory)
        {
        }

        #endregion
    }
}