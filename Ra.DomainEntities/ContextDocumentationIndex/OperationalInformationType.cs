namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class OperationalInformationType : EntityBase
    {
        #region Properties

        public virtual DocumentCategoryType Category { get; set; }

        public virtual bool OperationalSystemConvertedInformation { get; set; } = false;

        public virtual bool OperationalSystemInformation { get; set; } = false;

        public virtual bool OperationalSystemInformationOther { get; set; } = false;

        public virtual bool OperationalSystemSOA { get; set; } = false;

        #endregion
    }
}