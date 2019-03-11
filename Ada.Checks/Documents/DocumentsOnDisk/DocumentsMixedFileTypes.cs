namespace Ada.Checks.Documents.DocumentsOnDisk
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class DocumentsMixedFileTypes : AdaAvDynamicFromSql
    {
        #region  Constructors

        public DocumentsMixedFileTypes()
            : base("4.G_12")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT dID, relativePath as Path, COUNT(DISTINCT extension) as Count FROM 
                                    (SELECT dID, extension, relativePath FROM documentContentOnDisk WHERE extension != '.gml' and  extension NOT like '.xsd')
                                    GROUP BY dID HAVING COUNT(DISTINCT extension) != 1"
                ;
//
//
//
//            // give problems on gml-av's (ex 17050)
//            @"SELECT dID, relativePath, COUNT(DISTINCT extension) FROM documentContentOnDisk 
//												GROUP BY dID HAVING COUNT(DISTINCT extension) =1 and relativePath IN (SELECT relativePath FROM documents WHERE gmlXsd IS NULL)"
//                ;


//            // give problems on gml-av's (ex 17050)
//            @"SELECT dID, relativePath, COUNT(DISTINCT extension) FROM documentContentOnDisk 
//												GROUP BY dID HAVING COUNT(DISTINCT extension) =1 and relativePath IN (SELECT relativePath FROM documents WHERE gmlXsd IS NULL)"
//                ;

//             old old
//                                @"SELECT relativePath AS Path, COUNT(DISTINCT extension) FROM filesonDisk WHERE (SELECT rootFolder FROM media) || relativePath IN 
//												(SELECT fullPath FROM foldersOnDisk WHERE parent LIKE (SELECT rootFolder FROM media) || '\Documents\docCollection%') 
//												GROUP by Path HAVING COUNT(DISTINCT extension) > 1 AND Path IN (SELECT '\Documents\' || relativePath || '\' || dID FROM documents WHERE gmlXSD IS NULL)"
//                ;
        }

        #endregion
    }
}