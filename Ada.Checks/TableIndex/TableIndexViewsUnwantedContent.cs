namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexViewsUnwantedContent : AdaAvViolation
    {
        #region Static

        private static readonly List<string> UnwantedContentList = new List<string> {"DELETE ", "DROP ", "CREATE ", "INSERT "};

        #endregion

        #region  Constructors

        public TableIndexViewsUnwantedContent(View view)
            : base("6.C_18")
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
            if (UnwantedContentList.Any(x => view.QueryOriginal.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
                yield return new TableIndexViewsUnwantedContent(view);
        }

        #endregion
    }
}