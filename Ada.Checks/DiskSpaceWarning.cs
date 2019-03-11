namespace Ada.Checks
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class DiskSpaceWarning : AdaAvCheckNotification
    {
        #region  Constructors

        public DiskSpaceWarning(string driveName, double spaceNeeded)
            : base("0.3")
        {
            DriveName = driveName;
            SpaceNeeded = spaceNeeded;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string DriveName { get; set; }

        [AdaAvCheckNotificationTagInMb]
        public double SpaceNeeded { get; set; }

        #endregion
    }
}