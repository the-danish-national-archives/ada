namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class ArchiveIndexCaseIdOrCaseTitleMissing : AdaAvViolation
    {
        #region  Constructors

        public ArchiveIndexCaseIdOrCaseTitleMissing()
            : base("6.A_7")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<FunctionalDescription> functionalDescriptions)
        {
            if (!(functionalDescriptions.Contains(FunctionalDescription.Sagsidentifikation)
                  || functionalDescriptions.Contains(FunctionalDescription.Sagstitel)))
                yield return new ArchiveIndexCaseIdOrCaseTitleMissing();
        }

        #endregion
    }
}