namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Repositories;

    #endregion

    public class DocIndexDocsReferedFromTablesNotExisting : AdaAvViolation
    {
        #region  Constructors

        public DocIndexDocsReferedFromTablesNotExisting(long count, long orphans)
            : base("4.C.6_6")
        {
            Count = count;
            Orphans = orphans;
            OrphansPercentage = (float) orphans / count * 100;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public long Count { get; set; }

        [AdaAvCheckNotificationTag]
        public long Orphans { get; set; }

        [AdaAvCheckNotificationTagAsPercentage]
        public float OrphansPercentage { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(FullDocumentRepo fullDocRepo)
        {
            var tableCount = fullDocRepo.DocIdCountFromTables();
            var tableOrphans = fullDocRepo.DocIdOrphansFromTables();

            if (tableOrphans > 0) yield return new DocIndexDocsReferedFromTablesNotExisting(tableCount, tableOrphans);
        }

        #endregion
    }
}