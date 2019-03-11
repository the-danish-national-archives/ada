namespace Ra.DomainEntities.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public class Form : EntityBase
    {
        #region Properties

        public virtual ArchiveIndex ArchiveIndex { get; set; }

        public virtual IList<FormClass> Classes { get; set; }

        public virtual string FormVersion { get; set; }

        #endregion
    }
}