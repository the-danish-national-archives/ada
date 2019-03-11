namespace Ra.DomainEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public class ContextDocumentationIndex : EntityBase
    {
        #region Properties

        public virtual IList<ContextDocumentationDocument> Documents { get; set; }

        #endregion
    }
}