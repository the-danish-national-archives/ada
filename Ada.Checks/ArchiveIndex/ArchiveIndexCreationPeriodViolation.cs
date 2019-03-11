namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.ArchiveIndex;
    using Ra.EntityExtensions.ArchiveIndex;

    #endregion

    public class ArchiveIndexCreationPeriodViolation : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexCreationPeriodViolation(string startDate, string endDate)
            : base("6.A_3")
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
            foreach (var creator in index.ArchiveCreators)
                if (DateTime.Compare(creator.CreationPeriodEndAsDateTime(), creator.CreationPeriodStartAsDateTime()) < 0)
                    yield return new ArchiveIndexCreationPeriodViolation(creator.CreationPeriodStart, creator.CreationPeriodEnd);
        }

        #endregion
    }
}