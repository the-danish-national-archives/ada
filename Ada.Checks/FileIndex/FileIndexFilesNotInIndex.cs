namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FileIndexFilesNotInIndex : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FileIndexFilesNotInIndex()
            : base("4.C.2_2")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT relativePath || '\'  || name as Path FROM filesOnDisk WHERE entryKey NOT IN (SELECT entryKey FROM indexFolderContent WHERE name = 'fileIndex.xml')
												EXCEPT
												SELECT (media.name || relativePath || '\' || fileName||extension) FROM files INNER JOIN media WHERE files.mediaNumber = media.mediaNumber	";
        }

        #endregion
    }
}