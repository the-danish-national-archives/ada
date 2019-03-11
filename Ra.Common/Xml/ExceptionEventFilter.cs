namespace Ra.Common.Xml
{
    using System;
    using System.IO;
    using System.Xml;

    public interface IExceptionFilter
    {
        XmlHandlerEventArgs ProcessException(Exception e, FileInfo fileInfo);
    }

    public class ExceptionFilter : IExceptionFilter
    {
        public XmlHandlerEventArgs ProcessException(Exception e, FileInfo fileInfo)
        {
            if (e is XmlException || e is InvalidOperationException)
            {
                var args = new XmlHandlerEventArgs();

                args.EventType = XmlEventType.XmlWellFormednessError;
                args.OriginalMessage = e.Message;
                if (e is XmlException)
                {
                    args.Line = (e as XmlException).LineNumber;
                    args.Position = (e as XmlException).LinePosition;
                }

                if (e.InnerException != null)
                {
                    args.OriginalMessage += ": " + e.InnerException.Message;
                }

                args.fileInfo = fileInfo;
                return args;
            }
            else
            {
                throw e;
            }
        
        }         
    }
}
