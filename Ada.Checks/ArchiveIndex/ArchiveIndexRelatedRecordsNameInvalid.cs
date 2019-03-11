namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.ArchiveIndex;

    #endregion

    public class ArchiveIndexRelatedRecordsNameInvalid : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexRelatedRecordsNameInvalid(string record)
            : base("6.A_8")
        {
            Record = record;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Record { get; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ArchiveIndex index)
        {
            if (index.SearchRelatedOtherRecords)
                foreach (var name in index.RelatedRecordsName.Where(x => x.Length < 2))
                    yield return new ArchiveIndexRelatedRecordsNameInvalid(name);
        }

        #endregion
    }
}