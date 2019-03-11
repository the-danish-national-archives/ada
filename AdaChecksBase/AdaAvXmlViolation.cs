namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;
    using Ra.Common.Xml;

    #endregion

    public class AdaAvXmlViolation : AdaAvViolation
    {
        #region  Constructors

//        public AdaAvXmlViolation(string tagType)
//            : base(tagType)
//        {
//        }

//        public abstract AdaAvXmlViolation(object tagType)
//        {
//            
//        }

        protected AdaAvXmlViolation(XmlHandlerEventArgs args, string path)
            : base("0.2")
        {
        }

        protected AdaAvXmlViolation(string tagType, XmlHandlerEventArgs args, string path)
            : base(tagType)
        {
            Line = args.Line;
            Position = args.Position;
            XmlError = args.OriginalMessage;
            Path = path;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int Line { get; set; }

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        [AdaAvCheckNotificationTag]
        public int Position { get; set; }

        [AdaAvCheckNotificationTag]
        public string XmlError { get; set; }

        #endregion

        #region

        public static AdaAvXmlViolation CreateInstance(Type type, XmlHandlerEventArgs args, string path)
        {
            if (!type.IsSubclassOf(typeof(AdaAvXmlViolation)))
                throw new ArgumentOutOfRangeException(nameof(type), "Must be a sub class of AdaAvXmlViolation");

            return (AdaAvXmlViolation) Activator.CreateInstance(type, args, path);
            ;
        }

        #endregion
    }
}