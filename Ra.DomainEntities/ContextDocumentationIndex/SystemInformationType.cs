namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class SystemInformationType : EntityBase
    {
        #region Properties

        public virtual DocumentCategoryType Category { get; set; }

        public virtual bool SystemAdministrativeFunctions { get; set; } = false;

        public virtual bool SystemAgencyQualityControl { get; set; } = false;

        public virtual bool SystemContent { get; set; } = false;

        public virtual bool SystemDataProvision { get; set; } = false;

        public virtual bool SystemDataTransfer { get; set; } = false;

        public virtual bool SystemInformationOther { get; set; } = false;

        public virtual bool SystemPresentationStructure { get; set; } = false;

        public virtual bool SystemPreviousSubsequentFunctions { get; set; } = false;

        public virtual bool SystemPublication { get; set; } = false;

        public virtual bool SystemPurpose { get; set; } = false;

        public virtual bool SystemRegulations { get; set; } = false;

        #endregion
    }
}