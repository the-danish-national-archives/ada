namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.ArchiveIndex;
    using Ra.EntityExtensions.ArchiveIndex;

    #endregion

    public class ArchiveIndexArchivePeriodViolation : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexArchivePeriodViolation(string startDate, string endDate)
            : base("6.A_2")
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string EndDate { get; }

        [AdaAvCheckNotificationTag]
        public string StartDate { get; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ArchiveIndex index)
        {
            if (DateTime.Compare(index.ArchivePeriodEndAsDateTime(), index.ArchivePeriodStartAsDateTime()) < 0) yield return new ArchiveIndexArchivePeriodViolation(index.ArchivePeriodStart, index.ArchivePeriodEnd);
        }

        #endregion
    }
}