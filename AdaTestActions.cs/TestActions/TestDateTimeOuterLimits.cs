using System.Collections.Generic;

namespace Ada.ADA.Common.TestActions
{
    using System.IO;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    public class TestDateTimeOuterLimits: AdaActionBase<IAdaUowFactory>
    {
        protected override void OnRun(IAdaUowFactory testFactory)
        {
            var avUowFactory = new AdaAvUowFactory(this.Mapping.AVID, "av", new DirectoryInfo(Properties.Settings.Default.DBCreationFolder));

            using (var uow =testFactory.GetUnitOfWork())
            {
                using (var tableContentRepo = new TableContentRepo(avUowFactory, 1000))
                {
                    foreach (var table in uow.GetRepository<Table>().All())
                    {
                        foreach (var column in table.GetDateTimeColumns())
                        {
                            var minMax = tableContentRepo.GetMinMaxValues(column);
                            if (minMax != null)
                            {
                                var logEntry = new LogEntry { EntryTypeId = "4.D_11" };
                                logEntry.AddTag("TableName", column.TableName);
                                logEntry.AddTag("ColumnName", column.Name);
                                logEntry.AddTag("ColumnType", column.Type);
                                logEntry.AddTag("MinValue", minMax.Item1);
                                logEntry.AddTag("MaxValue", minMax.Item2);
                                this.ReportLogEntry(logEntry);
                            }
                        }
                    }
                }
            }
        }

        public TestDateTimeOuterLimits(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}
