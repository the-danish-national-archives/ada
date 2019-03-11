namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FileIndexFilesMissing : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FileIndexFilesMissing()
            : base("4.C.2_1")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT mediaNumber, Dir || '\' || File as Path FROM

                    (SELECT files.mediaNumber as mediaNumber, media.name || relativePath as Dir, fileName || extension as File FROM files INNER JOIN media WHERE files.mediaNumber = media.mediaNumber 
												EXCEPT
												SELECT mediaNumber, relativePath, name FROM filesOnDisk)";
        }

        #endregion
    }
}