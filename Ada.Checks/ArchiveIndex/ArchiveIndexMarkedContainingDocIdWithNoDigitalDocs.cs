namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class ArchiveIndexMarkedContainingDocIdWithNoDigitalDocs : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexMarkedContainingDocIdWithNoDigitalDocs()
            : base("6.C_14")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<FunctionalDescription> functionalDescriptions)
        {
            if (functionalDescriptions.Contains(FunctionalDescription.Dokumentidentifikation))
                yield return new ArchiveIndexMarkedContainingDocIdWithNoDigitalDocs();
        }

        #endregion
    }
}