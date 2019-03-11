namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ChecksBase;
    using log4net;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexViewSqlExecuted : AdaAvViolation
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region  Constructors

        public TableIndexViewSqlExecuted(string viewName, string sqlError)
            : base("6.C_12")
        {
            ViewName = viewName;
            SqlError = sqlError.Replace("\r\n", "¤");
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string SqlError { get; set; }

        [AdaAvCheckNotificationTag]
        public string ViewName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(UnitOfWork avUow, View view)
        {
            AdaAvCheckNotification onCatch = null;
            try
            {
                var command = avUow.Session.Connection.CreateCommand();
                command.CommandText = view.NotRunnableQuery;
                command.ExecuteNonQuery();

                PersistAvQuery(avUow, view);

            }
            catch (Exception ex)
            {
                onCatch = new TableIndexViewSqlExecuted(viewName: view.Name, sqlError: ex.Message);
            }

            return onCatch.YieldOrEmpty();
        }
        private static void PersistAvQuery(UnitOfWork avUow, View view)
        {
            if(!view.IsAvView()) return;
            try
            {
                var command = avUow.Session.Connection.CreateCommand();
                command.CommandText = $" Create table {view.Name}_persist as {view.QueryOriginal}";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error($"Could not persist {view.Name}",ex);
            }
        }

        #endregion
    }
}