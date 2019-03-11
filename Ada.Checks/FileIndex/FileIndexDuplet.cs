namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FileIndexDuplet : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FileIndexDuplet()
            : base("4.C.2_5")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"
    SELECT mediaNumber || relativePath || '\' || fileName || extension AS Path FROM 
        (SELECT mediaNumber, relativePath, fileName, extension, COUNT(1) as COUNT FROM files 
                GROUP BY mediaNumber, relativePath, fileName, extension)
        WHERE COUNT > 1";
        }

        #endregion
    }
}