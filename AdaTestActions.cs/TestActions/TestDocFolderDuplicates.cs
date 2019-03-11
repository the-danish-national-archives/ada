namespace Ada.ADA.Common.TestActions
{
    using System.IO;
    using System.Linq;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    public class TestDocFolderDuplicates : AdaActionBase<IAdaUowFactory>
    {
        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var repo = new AdaStructureRepo(factory,1000))
            {
                if (repo.EnumerateDuplicateDocFolders().Any())
                {
                    var logEntry = new LogEntry { EntryTypeId = "4.G_13" };
                    this.ReportLogEntry(logEntry);
                }
            }
        }

        public TestDocFolderDuplicates(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}
