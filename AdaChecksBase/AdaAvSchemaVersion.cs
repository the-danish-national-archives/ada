namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;

    #endregion

    public class AdaAvSchemaVersion : AdaAvViolation
    {
        #region  Constructors

        public AdaAvSchemaVersion(string tagType, string fileName, string version)
            : base(tagType)
        {
            FileName = fileName;
            Version = version;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string FileName { get; set; }

        [AdaAvCheckNotificationTag]
        public string Version { get; set; }

        #endregion

        #region

        public static AdaAvSchemaVersion CreateInstance(Type type, string fileName, string version)
        {
            var super = typeof(AdaAvSchemaVersion);
            if (!type.IsSubclassOf(super))
                throw new ArgumentOutOfRangeException(nameof(type), $"Must be a sub class of {super.AssemblyQualifiedName}");

            return (AdaAvSchemaVersion) Activator.CreateInstance(type, fileName, version);
        }

        #endregion
    }
}