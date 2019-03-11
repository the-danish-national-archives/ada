namespace Ra.DomainEntities.ContextDocumentationIndex
{
    public class InformationOtherType : EntityBase
    {
        #region Properties

        public virtual DocumentCategoryType Category { get; set; }
        public virtual bool InformationOther { get; set; } = false;

        #endregion
    }
}