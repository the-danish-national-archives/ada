namespace Ada.UI.Wpf.WorkspaceCleanUp.Model
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;

    #endregion

    public class FileSet
    {
        #region  Fields

        private readonly List<FileInfo> fileInfos;

        #endregion

        #region  Constructors

        public FileSet(string name, IEnumerable<FileInfo> fileInfos)
        {
            if (fileInfos == null)
                throw new ArgumentNullException(nameof(fileInfos));
            Name = name;
            this.fileInfos = fileInfos.ToList();
            Size += this.fileInfos.Aggregate(0L, (s, f) => s + f.Length);
        }

        #endregion

        #region Properties

        public string Name { get; }

        public long Size { get; }

        #endregion

        #region

        public bool DeleteItems()
        {
            var error = false;
            foreach (var fileInfo in fileInfos)
                try
                {
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                catch (IOException)
                {
                    error = true;
                }
                catch (SecurityException)
                {
                    error = true;
                }
                catch (UnauthorizedAccessException)
                {
                    error = true;
                }

            return !error;
        }

        #endregion
    }
}