namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class TableIndexNotifyViews : AdaAvCheckNotification
    {
        #region  Constructors

        public TableIndexNotifyViews(int viewCount, IList<string> listOfNames)
            : base("6.C_9")
        {
            ViewCount = viewCount;
            Count = listOfNames.Count;
            ListOfNames = listOfNames;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTagSmartToString(Seperator = "\n\t\t")]
        //"¤¤")]
        public IList<string> ListOfNames { get; set; }

        [AdaAvCheckNotificationTag]
        public int ViewCount { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Create(List<View> views)
        {
            var avViews = views.FindAll(x => x.IsAvQuery()).Select(x => x.Name).ToList();
            yield return new TableIndexNotifyViews(views.Count, avViews);
        }

        #endregion
    }
}