namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Common;
    using Ra.DomainEntities;

    #endregion

    public class FileIndexAvid : AdaAvViolation
    {
        #region  Constructors

        public FileIndexAvid(string path)
            : base("4.C.2_4")
        {
            Path = path;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(AVMapping mapping, string root, string path)
        {
            if (mapping.AVID.FullID != new AViD(root).FullID)
                yield return new FileIndexAvid(path);
        }

        #endregion
    }
}