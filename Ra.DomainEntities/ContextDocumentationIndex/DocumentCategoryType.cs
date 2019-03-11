namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class DocumentCategoryType : EntityBase
    {
        #region Properties

        public virtual ArchivalPreservationInformationType ArchivalPreservationInformation { get; set; }
        public virtual ContextDocumentationDocument Document { get; set; }

        public virtual InformationOtherType InformationOther { get; set; }

        public virtual IngestInformationType IngestInformation { get; set; }

        public virtual OperationalInformationType OperationalInformation { get; set; }

        public virtual SubmissionInformationType SubmissionInformation { get; set; }

        public virtual SystemInformationType SystemInformation { get; set; }

        #endregion
    }
}