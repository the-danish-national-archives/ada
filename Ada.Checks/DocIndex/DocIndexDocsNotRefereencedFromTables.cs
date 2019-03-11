namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;

    #endregion

    [AdaAvCheckToResultsList(nameof(OrphansList), ',')]
    public class DocIndexDocsNotRefereencedFromTables : AdaAvViolation
    {
        #region  Constructors

        public DocIndexDocsNotRefereencedFromTables(long count, IEnumerable<long> orphans)
            : base("4.C.6_5")
        {
            Count = count;
            Orphans = orphans.Count();
            OrphansPercentage = 100f * Orphans / count;
            OrphansList = orphans.Take(1000).Select(i => i.ToString()).ToList();
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public long Count { get; set; }

        [AdaAvCheckNotificationTag]
        public long Orphans { get; set; }

        [AdaAvCheckNotificationTagSmartToString(Seperator = ",", Hidden = true)]
        public IEnumerable<string> OrphansList { get; set; }

        [AdaAvCheckNotificationTagAsPercentage]
        public float OrphansPercentage { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(long docIdCount, IEnumerable<long> notEvenViaParent)
        {
            if (notEvenViaParent.Any())
                yield return new DocIndexDocsNotRefereencedFromTables(docIdCount, notEvenViaParent);
        }

        #endregion
    }
}