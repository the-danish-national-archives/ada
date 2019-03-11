//namespace Ada.ADA.Common.TestActions
//{
//    using System.IO;

//    using global::Ada.Common;
//    using global::Ada.Log;
//    using global::Ada.Log.Entities;

//    public class StreamPrimer: AdaActionBase
//    {
//        private FileInfo File;

//        private string notFoundErrorID;

//        public StreamPrimer(IAdaProcessLog processLog, IAdaTestLog testLog, FileInfo fileInfo, string notFoundErrorID)
//            : base(processLog, testLog)
//        {
//            this.File = fileInfo;
//            this.notFoundErrorID = notFoundErrorID;
//        }

//        protected override void OnRun(Job job)
//        {
//            if (!this.File.Exists)
//            {
//                var logEntry = new LogEntry { EntryTypeId = notFoundErrorID };
//                logEntry.AddTag("Name", this.File.FullName);
//                this.ReportLogEntry(logEntry);
//            }
//        }
//    }
//}
