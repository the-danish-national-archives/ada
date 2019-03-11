namespace Ada.ADA.Common.TestActions
{
    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    public class DocumentRelationTest : AdaActionBase<IAdaUowFactory>
    {
        protected override void OnRun(IAdaUowFactory factory)
        {
            long docIdxCount;
            long docIdxOrphans;
            long tableCount;
            long tableOrphans;

            using (var fullDocRepo = new FullDocumentRepo(factory, this.Mapping.AVID, Properties.Settings.Default.DBCreationFolder))
            {
                docIdxCount = fullDocRepo.DocIdCountFromDocIndex();
                docIdxOrphans = fullDocRepo.DocIdOrphansFromDocIndex();
                tableCount = fullDocRepo.DocIdCountFromTables();
                tableOrphans = fullDocRepo.DocIdOrphansFromTables();
            }

            var docErrorPercentage = (float)docIdxOrphans / docIdxCount * 100;
            var tableErrorPercentage = (float)tableOrphans / tableCount * 100;
            if (docIdxOrphans > 0)
            {
                var logEntry = new LogEntry { EntryTypeId = "4.C.6_5" };
                logEntry.AddTag("Count", docIdxCount.ToString());
                logEntry.AddTag("Orphans", docIdxOrphans.ToString());
                logEntry.AddTag("OrphansPercentage", docErrorPercentage.ToString("0.00"));
                this.ReportLogEntry(logEntry);
            }

            if (tableOrphans > 0)
            {
                var logEntry = new LogEntry { EntryTypeId = "4.C.6_6" };
                logEntry.AddTag("Count", tableCount.ToString());
                logEntry.AddTag("Orphans", tableOrphans.ToString());
                logEntry.AddTag("OrphansPercentage", tableErrorPercentage.ToString("0.00"));
                this.ReportLogEntry(logEntry);
            }
        }

        public DocumentRelationTest(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}

