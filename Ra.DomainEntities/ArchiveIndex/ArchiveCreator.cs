namespace Ra.DomainEntities.ArchiveIndex
{
    public class ArchiveCreator : EntityBase
    {
        #region Properties

        public virtual ArchiveIndex ArchiveIndex { get; set; }

        public virtual string CreationPeriodEnd { get; set; }

        public virtual string CreationPeriodStart { get; set; }

        public virtual string CreatorName { get; set; }

        #endregion
    }
}