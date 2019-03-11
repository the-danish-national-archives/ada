// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlValidationEventFilter.cs" company="">
//   
// </copyright>
// <summary>
//   The xml validation event filter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Ra.Common.Xml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using System.Linq;

    using NHibernate.Criterion;

    /// <summary>
    ///     The xml validation event filter.
    /// </summary>
    public class XmlValidationEventFilter : IXmlValidationFilter
    {
        #region Fields

        /// <summary>
        ///     The errors.
        /// </summary>
        private readonly Dictionary<string, long> errors = new Dictionary<string, long>();

        /// <summary>
        ///     The _total errors.
        /// </summary>
        private long _totalErrors;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The process event.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <param name="xmlFile">
        /// The xml file.
        /// </param>
        /// <returns>
        /// The <see cref="XmlHandlerEventArgs"/>.
        /// </returns>
        public XmlHandlerEventArgs ProcessEvent(ValidationEventArgs e, FileInfo sourceFileInfo)
        {
            string errorCode = e.Exception.GetErrorCode();
            this.UpdateErrorCount(errorCode);

            var eventData = new XmlHandlerEventArgs();

            eventData.EventCountTotal = this._totalErrors;
            eventData.EventCountInstance = this.GetErrorCount(errorCode);
            eventData.OriginalMessage = e.Exception.Message;
            eventData.FilteredMessage = e.Exception.Message;
            eventData.Inner = e.Exception;
            eventData.fileInfo = sourceFileInfo;

            eventData.EventCountInstance = this.GetErrorCount(errorCode);

            if (sourceFileInfo.Name.EndsWith(".xml"))
            {
                if (e.Severity == XmlSeverityType.Error)
                {
                    eventData.EventType = XmlEventType.XmlValidationError;
                }

                if (e.Severity == XmlSeverityType.Warning)
                {
                    eventData.EventType = XmlEventType.XmlValidationWarning;
                    if (errorCode == "Sch_NoElementSchemaFound" || errorCode == "Sch_NoAttributeSchemaFound"
                        || errorCode == "Sch_NoAttributeSchemaFound" || errorCode == "Sch_InvalidNamespace"
                        || errorCode == "Sch_InvalidTargetNamespaceAttribute"
                        || errorCode == "Sch_InvalidNamespaceAttribute")
                    {
                        eventData.FilteredMessage += "\n\tTest element og header for namespacefejl";
                    }
                }
            }

            if (sourceFileInfo.Name.EndsWith(".xsd"))
            {
                if (e.Severity == XmlSeverityType.Error)
                {


                    eventData.EventType = XmlEventType.SchemaValidationError;
                    //XmlQualifiedName name = new XmlQualifiedName("http://www.opengis.net/gml");

                    //if (!e.Exception.SourceSchemaObject.Namespaces.ToArray().Contains(name))
                    //{
                    //    eventData.FilteredMessage += "¤Manglende namespace: " + name.ToString();

                    //}
                    
                }

                if (e.Severity == XmlSeverityType.Warning)
                {
                    eventData.EventType = XmlEventType.SchemaValidationWarning;                   
                }


            }

            return eventData;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get error count.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        private long GetErrorCount(string errorCode)
        {
            return this.errors[errorCode];
        }

        /// <summary>
        /// The update error count.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        private void UpdateErrorCount(string errorCode)
        {
            this._totalErrors++;
            if (!this.errors.ContainsKey(errorCode))
            {
                this.errors.Add(errorCode, 1);
            }
            else
            {
                this.errors[errorCode]++;
            }
        }

        #endregion
    }
}