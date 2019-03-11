namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities;
    using Ra.DomainEntities.ArchiveIndex;

    #endregion

    public class ArchiveVersionViolation : AdaAvViolation
    {
        #region  Constructors

        public ArchiveVersionViolation(string actualId, string annotatedId)
            : base("6.A_1")
        {
            ActualId = actualId;
            AnnotatedId = annotatedId;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag(Name = "ActualId")]
        public string ActualId { get; }


        [AdaAvCheckNotificationTag]
        public string AnnotatedId { get; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ArchiveIndex index, AViD AVID)
        {
            if (
                !string.Equals(
                    index.ArchiveInformationPackageId,
                    AVID.FullID,
                    StringComparison.OrdinalIgnoreCase))
            {
                var violation = new ArchiveVersionViolation(
                    AVID.FullID,
                    index.ArchiveInformationPackageId);
                yield return violation;
            }
        }

        #endregion
    }
}