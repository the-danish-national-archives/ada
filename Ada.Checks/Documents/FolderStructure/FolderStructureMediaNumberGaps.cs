namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class FolderStructureDuplicateMediaNumber : AdaAvViolation //: AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureDuplicateMediaNumber(string path, long count)
            : base("4.B.1_2")
        {
            Path = path;
            Count = count;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public long Count { get; set; }

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion


        //        protected override string GetTestQuery(Type type)
        //        {
        //            return
        //                @"SELECT name AS Path, COUNT(mediaNumber) as Count from media GROUP BY mediaNumber HAVING COUNT(mediaNumber)>1"
        //                ;
        //        }
    }
}