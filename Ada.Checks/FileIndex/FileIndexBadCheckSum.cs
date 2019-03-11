namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FileIndexBadCheckSum : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FileIndexBadCheckSum()
            : base("4.C.2_3")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return @"SELECT * FROM (SELECT files.mediaNumber as mediaNumber, media.name || relativePath || '\' || fileName || extension as Path, md5 as MD5 FROM files INNER JOIN media WHERE files.mediaNumber = media.mediaNumber 
												EXCEPT 
												SELECT mediaNumber, relativePath || '\'  || name , checksum  FROM filesOnDisk) WHERE EXISTS (SELECT checksum FROM filesOnDisk WHERE checksum NOT NULL)";
        }

        #endregion
    }
}