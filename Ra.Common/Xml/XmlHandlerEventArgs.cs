// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlHandlerEventArgs.cs" company="">
//   
// </copyright>
// <summary>
//   The xml event handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    #endregion

    /// <summary>
    ///     The xml event handler.
    /// </summary>
    /// <param name="sender">
    ///     The sender.
    /// </param>
    /// <param name="e">
    ///     The log event.
    /// </param>
    public delegate void XmlEventHandler(object sender, XmlHandlerEventArgs e);

    /// <summary>
    ///     The xml handler event args.
    /// </summary>
    public class XmlHandlerEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlHandlerEventArgs" /> class.
        /// </summary>
        public XmlHandlerEventArgs(XmlEventType eventType, FileInfo sourceFileInfo, Exception e = null)
        {
            fileInfo = sourceFileInfo;
            EventType = eventType;
            if (e != null)
            {
                var NillableMessage =
                    "If the 'nillable' attribute is false in the schema, the 'xsi:nil' attribute must not be present in the instance.";
                if (e.Message.Equals(NillableMessage))
                {
                    Line = (e as XmlSchemaException).LineNumber;
                    Position = (e as XmlSchemaException).LinePosition;
                    e = new XmlException(NillableMessage, e, Line, Position);
                }

                NillableMessage =
                    "Hvis attributten 'nillable' er 'false' i skemaet, må attributten 'xsi:nil' ikke findes i forekomsten.";
                if (e.Message.Equals(NillableMessage))
                {
                    Line = (e as XmlSchemaException).LineNumber;
                    Position = (e as XmlSchemaException).LinePosition;
                    e = new XmlException(NillableMessage, e, Line, Position);
                }
            }

            FilteredMessage = string.Empty;
            OriginalMessage = string.Empty;

            if (e != null)
            {
                OriginalException = e;
                OriginalMessage = e.Message;
                if (e.InnerException != null && !(e is XmlException || e is XmlSchemaException)) OriginalMessage += " - " + e.InnerException.Message;

                if (e is XmlException)
                {
                    Line = (e as XmlException).LineNumber;
                    Position = (e as XmlException).LinePosition;
                }

                if (e is XmlSchemaException)
                {
                    Line = (e as XmlSchemaException).LineNumber;
                    Position = (e as XmlSchemaException).LinePosition;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the event count instance.
        /// </summary>
        public long EventCountInstance { get; set; }

        /// <summary>
        ///     Gets or sets the event count total.
        /// </summary>
        public long EventCountTotal { get; set; }

        /// <summary>
        ///     Gets or sets the event type.
        /// </summary>
        public XmlEventType EventType { get; }

        public FileInfo fileInfo { get; }

        /// <summary>
        ///     Gets or sets the filtered message.
        /// </summary>
        public string FilteredMessage { get; set; }

        /// <summary>
        ///     Gets or sets the line.
        /// </summary>
        public int Line { get; }

        public Exception OriginalException { get; set; }

        /// <summary>
        ///     Gets or sets the original message.
        /// </summary>
        public string OriginalMessage { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public int Position { get; }

        #endregion
    }
}