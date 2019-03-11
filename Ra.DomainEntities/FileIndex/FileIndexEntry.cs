namespace Ra.DomainEntities.FileIndex
{
    public class FileIndexEntry : EntityBase
    {
        #region Properties

        public virtual string Extension { get; set; }
        public virtual string FileName { get; set; }
        public virtual string foN { get; set; }
        public virtual string Md5 { get; set; }
        public virtual string MediaNumber { get; set; }
        public virtual string RelativePath { get; set; }

        #endregion
    }
}