namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;

    #endregion

    public class AdaAvGmlViolation : AdaAvViolation
    {
        #region  Constructors

        protected AdaAvGmlViolation(string tagType, string path)
            : base(tagType)
        {
            Path = path;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion

        #region

        public static AdaAvGmlViolation CreateInstance(Type type, string path)
        {
            if (!type.IsSubclassOf(typeof(AdaAvGmlViolation)))
                throw new ArgumentOutOfRangeException(nameof(type), "Must be a sub class of AdaAvGmlViolation");

            return (AdaAvGmlViolation) Activator.CreateInstance(type, path);
            ;
        }

        #endregion
    }
}