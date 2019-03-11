namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class ArchiveIndexDocDateMissing : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexDocDateMissing()
            : base("6.C_23")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<FunctionalDescription> functionalDescriptions)
        {
            if (!functionalDescriptions.Contains(FunctionalDescription.Dokumentdato))
                yield return new ArchiveIndexDocDateMissing();
        }

        #endregion
    }
}