namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Repositories;

    #endregion

    [AdaAvCheckToResultsList(nameof(OrphansList), ';')]
    public class DocIndexMissingParentId : AdaAvViolation
    {
        #region  Constructors

        public DocIndexMissingParentId(long count, List<(string dID, string pID)> parentOrphans)
            : base("4.C.6_3")
        {
            Count = count;
            Orphans = parentOrphans.Count();
            OrphansPercentage = 100f * Orphans / count;
            OrphansList = parentOrphans.Take(1000).Select(i => i.ToString()).ToList();
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public long Count { get; set; }

        [AdaAvCheckNotificationTag]
        public long Orphans { get; set; }

        [AdaAvCheckNotificationTagSmartToString(Seperator = ";", Hidden = true)]
        public IEnumerable<string> OrphansList { get; set; }

        [AdaAvCheckNotificationTagAsPercentage]
        public float OrphansPercentage { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(DocumentIndexRepo repo)
        {
            var refs = repo.NonExistentParentIds().ToList();
            if (refs.Any())
                yield return new DocIndexMissingParentId(repo.TotalDocumentCount(), refs);
        }

        #endregion
    }
}