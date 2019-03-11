namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class ArchivalPreservationInformationType : EntityBase
    {
        #region Properties

        public virtual bool ArchivalInformationOther { get; set; } = false;

        public virtual bool ArchivalMigrationInformation { get; set; } = false;
        public virtual DocumentCategoryType Category { get; set; }

        #endregion
    }
}