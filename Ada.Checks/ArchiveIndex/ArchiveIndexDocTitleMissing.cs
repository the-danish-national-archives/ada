namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class ArchiveIndexDocTitleMissing : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexDocTitleMissing()
            : base("6.A_6")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<FunctionalDescription> functionalDescriptions)
        {
            if (!functionalDescriptions.Contains(FunctionalDescription.Dokumenttitel))
                yield return new ArchiveIndexDocTitleMissing();
        }

        #endregion
    }
}