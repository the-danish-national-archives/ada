namespace Ra.DomainEntities.FileSystem
{
    public class FileSystemFile : FileSystemEntry
    {
        #region Properties

        public virtual string CheckSum { get; set; }

        public virtual string Extension { get; set; }

        public virtual long Size { get; set; }

        #endregion
    }
}