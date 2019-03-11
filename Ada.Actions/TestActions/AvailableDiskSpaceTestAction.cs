namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using System;
    using System.IO;
    using ActionBase;
    using Checks;
    using Checks.Documents.FolderStructure;
    using ChecksBase;
    using Common;
    using Log;
    using Ra.Common.Repository.NHibernate;
    using Repositories;

    #endregion

    [AdaActionPrecondition]
    [ReportsChecks(typeof(DiskSpaceWarning))]
    [RequiredChecks(typeof(FolderStructureDuplicateMediaNumber))]
    public class AvailableDiskSpaceTestAction : AdaActionBase<AvailableDiskSpaceTestAction.DBRootDrive>
    {
        #region  Fields

        private readonly IAdaUowFactory testFactory;

        #endregion

        #region  Constructors

        public AvailableDiskSpaceTestAction(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
        }

        #endregion

        #region

        protected override void OnRun(DBRootDrive driveroot)
        {
            var drive = new DriveInfo(driveroot.Path);
            var freeSpace = drive.AvailableFreeSpace;
            long tableSizeTotal;

            using (var uow = (UnitOfWork) testFactory.GetUnitOfWork())
            {
                var comm = uow.Session.Connection.CreateCommand();
                comm.CommandText =
                    "SELECT SUM(size) as tableSize FROM FilesOnDisk WHERE extension='.xml' COLLATE NOCASE";
                var res = comm.ExecuteScalar();

                tableSizeTotal = res == DBNull.Value ? 0 : Convert.ToInt64(res);
            }

            if (freeSpace < tableSizeTotal)
            {
                var spaceNeeded = Math.Round((float) tableSizeTotal / 1000000, 3);

                Report(new DiskSpaceWarning(drive.Name, spaceNeeded));
            }
        }

        #endregion

        #region Nested type: DBRootDrive

        public class DBRootDrive
        {
            #region  Constructors

            public DBRootDrive()
            {
            }

            public DBRootDrive(string path)
            {
                Path = path;
            }

            #endregion

            #region Properties

            public string Path { get; }

            #endregion
        }

        #endregion
    }
}