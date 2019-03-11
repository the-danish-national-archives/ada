namespace Ada.ADA.Common.TestActions
{
    using System;
    using System.IO;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Properties;
    using global::Ada.Repositories;

    using Ra.Common.Repository.NHibernate;

    public class TestAvailableDiskSpace : AdaActionBase<string>
    {
        private readonly IAdaUowFactory testFactory;

        protected override void OnRun(string driveroot)
        {
           
            var drive = new DriveInfo(driveroot);
            var freeSpace = drive.AvailableFreeSpace;
            long tableSizeTotal;

            using (var uow = (UnitOfWork)this.testFactory.GetUnitOfWork())
            {
                var comm = uow.Session.Connection.CreateCommand();
                comm.CommandText =
                    "SELECT SUM(size) as tableSize FROM FilesOnDisk WHERE extension='.xml' COLLATE NOCASE";
                tableSizeTotal = Convert.ToInt64(comm.ExecuteScalar());
            }

            if (freeSpace < tableSizeTotal)
            {
                var spaceNeeded = Math.Round((float)tableSizeTotal / 1000000, 3);
                var logEntry = new LogEntry { EntryTypeId = "0.3" };
                logEntry.AddTag("DriveName", drive.Name);
                logEntry.AddTag("SpaceNeeded", spaceNeeded.ToString());
                this.ReportLogEntry(logEntry);
            }
        }

        public TestAvailableDiskSpace(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
        }
    }
}
