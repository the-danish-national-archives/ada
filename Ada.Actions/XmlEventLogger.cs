namespace Ada.Actions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ActionBase;
    using ChecksBase;
    using Common;
    using Ra.Common.Xml;

    #endregion

    public interface IXmlEventLogger
    {
        #region Properties

        Action<AdaAvXmlViolation> CallBack { get; set; }

        #endregion

        #region

        void EventHandler(object sender, XmlHandlerEventArgs args);

        #endregion
    }

    public class XmlEventLogger : IXmlEventLogger
    {
        #region  Fields

        private readonly Dictionary<XmlEventType, Type> _errorLookup;

        private readonly AVMapping avMapping;

        #endregion

        #region  Constructors

        public XmlEventLogger(Dictionary<XmlEventType, Type> errorLookup, AVMapping avMapping)
        {
            _errorLookup = errorLookup;
            this.avMapping = avMapping;
        }

        #endregion

        #region IXmlEventLogger Members

        public Action<AdaAvXmlViolation> CallBack { get; set; }

        public void EventHandler(object sender, XmlHandlerEventArgs args)
        {
            if (!_errorLookup.ContainsKey(args.EventType))
                throw new InvalidOperationException();

            CallBack?.Invoke(
                AdaAvXmlViolation.CreateInstance(
                    _errorLookup[args.EventType],
                    args,
                    avMapping.GetRelativePath(args.fileInfo)));

            if (args.EventType == XmlEventType.Exception
                || args.EventType == XmlEventType.XmlWellFormednessError
                || args.EventType == XmlEventType.SchemaWellFormedNessError
                || args.EventType == XmlEventType.SchemaValidationError
                || args.EventType == XmlEventType.XmlValidationError)
                throw new AdaSkipActionException(args.OriginalMessage, args.OriginalException);
        }

        #endregion
    }
}