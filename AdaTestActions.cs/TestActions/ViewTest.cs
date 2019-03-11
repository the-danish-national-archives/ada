namespace Ada.ADA.Common.TestActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.ExtensionMethods;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.TableIndex;
    using Ra.DomainEntities.TableIndex.Extensions;

    public class ViewTest : AdaActionBase<IAdaUowFactory>
    {


        private readonly IAdaUowFactory avFactory;

        protected override void OnRun(IAdaUowFactory factory)
        {
            List<View> views;

            using (var uow = (UnitOfWork)factory.GetUnitOfWork())
            {
                views = uow.GetRepository<View>().All().ToList();
            }

            using (var uow = (UnitOfWork)avFactory.GetUnitOfWork())
            {
                var avViews = views.FindAll(x => x.IsAvQuery()).Select(x => x.Name).ToList();
                var logEntry = new LogEntry { EntryTypeId = "6.C_9" };
                logEntry.AddTag("ViewCount", views.Count.ToString());
                logEntry.AddTag("AvViewCount", avViews.Count().ToString());
                logEntry.AddTag("ListOfNames", avViews.SmartToString("¤¤"));
                this.ReportLogEntry(logEntry);

                var duplicateNames =
                    views.GroupBy(x => x.Name).Where(group => group.Count() > 1).Select(group => group.Key);
                foreach (var name in duplicateNames)
                {
                    logEntry = new LogEntry { EntryTypeId = "6.C_10" };
                    logEntry.AddTag("ViewName", name);
                    this.ReportLogEntry(logEntry);
                }

                foreach (var view in views)
                {
                    if (view.IsAvQuery())
                    {
                        if (view.HasLackingDescription())
                        {
                            logEntry = new LogEntry { EntryTypeId = "6.C_8" };
                            logEntry.AddTag("ViewName", view.Name);
                            this.ReportLogEntry(logEntry);
                        }

                        if (view.HasUnwantedContent())
                        {
                            logEntry = new LogEntry { EntryTypeId = "6.C_18" };
                            logEntry.AddTag("ViewName", view.Name);
                            this.ReportLogEntry(logEntry);
                        }
                        else
                        {
                            try
                            {
                                var command = uow.Session.Connection.CreateCommand();
                                command.CommandText = view.QueryOriginal;
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                logEntry = new LogEntry { EntryTypeId = "6.C_12" };
                                logEntry.AddTag("ViewName", view.Name);
                                logEntry.AddTag("SqlError", ex.Message.Replace("\r\n", "¤"));
                                this.ReportLogEntry(logEntry);
                            }
                        }
                    }
                }
            }
        }

        public ViewTest(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory avFactory, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.avFactory = avFactory;
        }
    }
}