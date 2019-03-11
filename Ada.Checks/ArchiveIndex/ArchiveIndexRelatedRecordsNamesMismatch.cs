namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.ArchiveIndex;

    #endregion

    public class ArchiveIndexRelatedRecordsNamesMisMatch : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexRelatedRecordsNamesMisMatch()
            : base("6.A_9")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ArchiveIndex index)
        {
            if (!index.SearchRelatedOtherRecords && index.RelatedRecordsName.Any()) yield return new ArchiveIndexRelatedRecordsNamesMisMatch();
        }

        #endregion
    }
}