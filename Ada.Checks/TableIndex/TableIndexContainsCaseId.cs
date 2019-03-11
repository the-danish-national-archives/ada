namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexContainsCaseId : AdaAvViolation
    {
        #region  Constructors

        public TableIndexContainsCaseId(FunctionalDescription functionalDescription)
            : base("6.C_13")
        {
            FunctionalDescription = functionalDescription;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public FunctionalDescription FunctionalDescription { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<FunctionalDescription> functionalDescriptions)
        {
            if (functionalDescriptions.Contains(FunctionalDescription.Sagsidentifikation)) yield return new TableIndexContainsCaseId(FunctionalDescription.Sagsidentifikation);

            if (functionalDescriptions.Contains(FunctionalDescription.Sagstitel)) yield return new TableIndexContainsCaseId(FunctionalDescription.Sagstitel);
        }

        #endregion
    }
}