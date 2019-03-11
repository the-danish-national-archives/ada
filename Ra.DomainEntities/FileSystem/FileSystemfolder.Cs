namespace Ra.DomainEntities.FileSystem
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public class FileSystemFolder : FileSystemEntry
    {
        #region Properties

        public virtual IList<FileSystemEntry> Contents { get; set; } = new List<FileSystemEntry>();

        public virtual int FileCount { get; set; }

        public virtual int FolderCount { get; set; }

        #endregion
    }
}