namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexViewsMissingDescription : AdaAvViolation
    {
        #region  Constructors

        public TableIndexViewsMissingDescription(View view)
            : base("6.C_8")
        {
            ViewName = view.Name;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ViewName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(View view)
        {
            if (view.Description == null || view.Description?.Length <= 2)
                yield return new TableIndexViewsMissingDescription(view);
        }

        #endregion
    }
}