namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class SubmissionInformationType : EntityBase
    {
        #region Properties

        public virtual bool ArchivalInformationOther { get; set; } = false;

        public virtual bool ArchivalProvisions { get; set; } = false;

        public virtual bool ArchivalTransformationInformation { get; set; } = false;
        public virtual DocumentCategoryType Category { get; set; }

        #endregion
    }
}