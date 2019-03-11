namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class ContextDocumentationIndexDocumentAuthor : EntityBase
    {
        #region Properties

        public virtual string AuthorInstitution { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual ContextDocumentationDocument Document { get; set; }

        #endregion
    }
}