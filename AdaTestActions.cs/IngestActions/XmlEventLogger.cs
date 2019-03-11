using Ra.Common.Xml;
using System.Collections.Generic;

namespace Ada.Common.IngestActions
{
    using Ada.Entities.AdaAvCheckNotifications;
    using System;

    public interface IXmlEventLogger
    {
        void EventHandler(object sender, XmlHandlerEventArgs args); 
    }

    public class XmlEventLogger : IXmlEventLogger
    {

        private readonly Action<AdaAvXmlViolation> _callBack;

        private readonly Dictionary<XmlEventType, Type> _errorLookup;

        private readonly AVMapping avMapping;

        public XmlEventLogger(Action<AdaAvXmlViolation> callBack, Dictionary<XmlEventType, Type> errorLookup, AVMapping avMapping)
        {
            _callBack = callBack;
            _errorLookup = errorLookup;
            this.avMapping = avMapping;
        }

        public void EventHandler(object sender, XmlHandlerEventArgs args)
        {


            if (!_errorLookup.ContainsKey(args.EventType))
                throw new InvalidOperationException();

            _callBack(
                AdaAvXmlViolation.CreateInstance(
                    _errorLookup[args.EventType],
                    args: args,
                    path: this.avMapping.GetRelativePath(args.fileInfo)));

            if (args.EventType == XmlEventType.Exception 
                || args.EventType == XmlEventType.XmlWellFormednessError 
                || args.EventType == XmlEventType.SchemaWellFormedNessError 
                || args.EventType == XmlEventType.SchemaValidationError
                || args.EventType == XmlEventType.XmlValidationError)
            {
                throw new AdaSkipActionException(args.OriginalMessage, args.OriginalException);
            }
        }
    }
}