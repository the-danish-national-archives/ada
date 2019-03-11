namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class IngestInformationType : EntityBase
    {
        #region Properties

        public virtual bool ArchivalInformationOther { get; set; } = false;

        public virtual bool ArchivalTestNotes { get; set; } = false;

        public virtual bool ArchivistNotes { get; set; } = false;
        public virtual DocumentCategoryType Category { get; set; }

        #endregion
    }
}