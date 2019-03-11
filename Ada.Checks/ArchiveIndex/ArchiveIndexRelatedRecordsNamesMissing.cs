namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.ArchiveIndex;

    #endregion

    public class ArchiveIndexRelatedRecordsNamesMissing : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexRelatedRecordsNamesMissing()
            : base("6.A_8_1")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ArchiveIndex index)
        {
            if (index.SearchRelatedOtherRecords && !index.RelatedRecordsName.Any()) yield return new ArchiveIndexRelatedRecordsNamesMissing();
        }

        #endregion
    }
}