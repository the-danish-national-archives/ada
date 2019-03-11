namespace Ra.DomainEntities.FileSystem
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class FileSystemEntry : EntityBase
    {
        #region Properties

        public virtual IList<FileSystemFolder> Ancestors { get; set; } = new List<FileSystemFolder>();

        public virtual int Level { get; set; }

        public virtual int MediaNumber { get; set; }
        public virtual string Name { get; set; }

        public virtual string RelativePath { get; set; }

        public virtual DateTime TimeStamp { get; set; }

        #endregion
    }
}