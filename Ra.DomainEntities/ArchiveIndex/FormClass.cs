namespace Ra.DomainEntities.ArchiveIndex
{
    public class FormClass : EntityBase
    {
        #region Properties

        public virtual string Class { get; set; }

        public virtual Form Form { get; set; }

        public virtual string Text { get; set; }

        #endregion
    }
}