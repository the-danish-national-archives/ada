namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexDuplikateViewName : AdaAvViolation
    {
        #region  Constructors

        public TableIndexDuplikateViewName(string viewName)
            : base("6.C_10")
        {
            ViewName = viewName;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ViewName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(IEnumerable<View> views)
        {
            foreach (var name in views.GroupBy(x => x.Name).Where(group => group.Count() > 1).Select(group => group.Key)) yield return new TableIndexDuplikateViewName(name);
//            "SELECT COUNT(viewName) AS Count, viewName as ViewName FROM views GROUP BY viewName HAVING COUNT(viewName)>1"
        }

        #endregion
    }
}