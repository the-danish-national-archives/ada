namespace Ra.DomainEntities.Documents
{
    public class DocIndexEntry : EntityBase
    {
        #region Properties

        public virtual string DocumentFolder { get; set; }
        public virtual string DocumentId { get; set; }
        public virtual string Extension { get; set; }
        public virtual string GmlXsd { get; set; }
        public virtual string MediaId { get; set; }
        public virtual string OriginalFileName { get; set; }
        public virtual string ParentId { get; set; }
        public virtual string SubmissionFileType { get; set; }

        #endregion
    }
}