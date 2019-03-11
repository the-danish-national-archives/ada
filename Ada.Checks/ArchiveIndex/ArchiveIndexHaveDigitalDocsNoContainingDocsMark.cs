namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class ArchiveIndexHaveDigitalDocsNoContainingDocsMark : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexHaveDigitalDocsNoContainingDocsMark()
            : base("6.C_14_1")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<FunctionalDescription> functionalDescriptions)
        {
            if (!functionalDescriptions.Contains(FunctionalDescription.Dokumentidentifikation))
                yield return new ArchiveIndexHaveDigitalDocsNoContainingDocsMark();
        }

        #endregion
    }
}