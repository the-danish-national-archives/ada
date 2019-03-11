namespace Ada.Checks.Documents.DocumentsOnDisk
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class DocumentsMissingDoc : AdaAvDynamicFromSql
    {
        #region  Constructors

        public DocumentsMissingDoc()
            : base("4.G_6")
        {
        }

        #endregion

        #region Properties

        private static string contentOnDisk => "documentContentOnDisk";

        private static string Documents => "documents";

        #endregion

        #region

        [AdaAvCheckToTestQuery]
        public static string GetDataTable()
        {
            return
                $@"SELECT dID, medieNumber as mID, relativePath as dCf  from {Documents}
        WHERE dID NOT IN (SELECT dID FROM {contentOnDisk})"
                ;
        }


        protected override string GetTestQuery(Type type)
        {
//            return
//                $@"
//SELECT Count FROM 
//	(SELECT COUNT(dID) as Count from {Documents}
//        WHERE dID NOT IN (SELECT dID FROM {contentOnDisk}))
//	WHERE Count <> 0"

            return
                $@"
SELECT Count FROM 
	(SELECT COUNT(dID) as Count from {Documents}
        WHERE dID NOT IN (SELECT dID FROM {contentOnDisk}))
	WHERE Count <> 0"
                ;
        }

        #endregion
    }
}