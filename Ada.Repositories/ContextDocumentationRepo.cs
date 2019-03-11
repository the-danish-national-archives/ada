namespace Ada.Repositories
{
    #region Namespace Using

    using Ra.DomainEntities.ContextDocumentationIndex;

    #endregion

    public class ContextDocumentationRepo : IngestEntityRepoBase<ContextDocumentationIndex>
    {
        #region  Constructors

        public ContextDocumentationRepo(IAdaUowFactory factory)
            : base(factory)
        {
        }

        #endregion
    }
}