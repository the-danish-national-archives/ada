using System.Collections.Generic;

namespace Ada.ADA.Common.TestActions
{
    namespace Ada.ADA.Common.TestActions
    {
        using System.IO;

        using global::Ada.Common;
        using global::Ada.Log;
        using global::Ada.Log.Entities;
        using global::Ada.Repositories;

        public class DocIndexIntegrityCheck : AdaActionBase<IAdaUowFactory>
        {
            protected override void OnRun(IAdaUowFactory factory)
            {
                using (var repo = new DocumentIndexRepo(
                            factory,
                            1000))
                {
                    long docCount = repo.TotalDocumentCount();
                    long nestedDocCount = repo.NestedDocumentCount();
                    float nestedPercentage = (float)nestedDocCount / docCount * 100;

                    var logEntry = new LogEntry { EntryTypeId = "4.C.6_1" };
                    logEntry.AddTag("DocCount", docCount.ToString());
                    logEntry.AddTag("NestedDocCount", nestedDocCount.ToString());
                    logEntry.AddTag("Percentage", nestedPercentage.ToString("0.00"));
                    this.ReportLogEntry(logEntry);


                    foreach (var missingref in repo.NonExistentParentIds())
                    {
                        logEntry = new LogEntry { EntryTypeId = "4.C.6_3" };
                        logEntry.AddTag("dID", missingref.Item1);
                        logEntry.AddTag("pID", missingref.Item2);
                        this.ReportLogEntry(logEntry);
                    }

                    foreach (var recursiveRef in repo.GetRecursiveIds())
                    {
                        logEntry = new LogEntry { EntryTypeId = "4.C.6_4" };
                        logEntry.AddTag("dID", recursiveRef);
                        this.ReportLogEntry(logEntry);
                    }
                }         
            }

            public DocIndexIntegrityCheck(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
                : base(processLog, testLog, mapping)
            {
            }
        }
    }
}
