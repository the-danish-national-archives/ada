namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureMediaNumberGaps : AdaAvDynamicFromSql
    {
        #region  Constructors

        protected FolderStructureMediaNumberGaps()
            : base("4.B.1_3")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"
SELECT mediaNumber + 1 AS Gap FROM media
                    EXCEPT SELECT MAX(mediaNumber) + 1 AS Gap FROM media
                    EXCEPT SELECT mediaNumber AS Gap FROM media
 ORDER BY Gap
"

// Very fast but creates a temporary table
                //                @"
                //DROP TABLE IF EXISTS MediaTemp;
                //CREATE TEMPORARY  TABLE MediaTemp AS SELECT * FROM media ;
                //
                //SELECT a.mediaNumber + 1 AS Gap FROM MediaTemp a WHERE NOT EXISTS (SELECT * FROM MediaTemp b WHERE a.mediaNumber + 1 = b.mediaNumber) 
                //					EXCEPT SELECT MAX(c.mediaNumber) + 1 AS Gap FROM MediaTemp c ORDER BY Gap"

                // This one is very slow on sqlite when there's many files
                //                                @"SELECT a.mediaNumber + 1 AS Gap FROM media a WHERE NOT EXISTS (SELECT * FROM media b WHERE a.mediaNumber + 1 = b.mediaNumber)   
                //                												 EXCEPT SELECT MAX(mediaNumber) + 1 AS Gap FROM media ORDER BY Gap"
                ;
        }

        #endregion
    }
}