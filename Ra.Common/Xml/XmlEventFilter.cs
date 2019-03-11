// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlEventFilter.cs" company="">
//   
// </copyright>
// <summary>
//   The xml validation event filter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;

    #endregion

    /// <summary>
    ///     The xml validation event filter.
    /// </summary>
    public class XmlEventFilter : IXmlEventFilter
    {
        #region  Fields

        /// <summary>
        ///     The errors.
        /// </summary>
        private readonly Dictionary<string, long> errors = new Dictionary<string, long>();

        /// <summary>
        ///     The _total errors.
        /// </summary>
        private long _totalErrors;

        #endregion

        #region IXmlEventFilter Members

        /// <summary>
        ///     The process event.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        /// <param name="xmlFile">
        ///     The xml file.
        /// </param>
        /// <returns>
        ///     The <see cref="XmlHandlerEventArgs" />.
        /// </returns>
        public XmlHandlerEventArgs ProcessEventArgs(XmlHandlerEventArgs args)
        {
            var errorCode = "Other";
            if (args.OriginalException is XmlSchemaException) errorCode = (args.OriginalException as XmlSchemaException).GetErrorCode();

            UpdateErrorCount(errorCode);

            args.EventCountTotal = _totalErrors;
            args.EventCountInstance = GetErrorCount(errorCode);

            if (args.EventType == XmlEventType.XmlValidationWarning)
                if (errorCode == "Sch_NoElementSchemaFound" || errorCode == "Sch_NoAttributeSchemaFound"
                                                            || errorCode == "Sch_NoAttributeSchemaFound" || errorCode == "Sch_InvalidNamespace"
                                                            || errorCode == "Sch_InvalidTargetNamespaceAttribute"
                                                            || errorCode == "Sch_InvalidNamespaceAttribute")
                    args.FilteredMessage += "\n\tTest element og header for namespacefejl";
            return args;
        }

        public bool ShouldRethrow(Exception e)
        {
            return !(e is XmlException) && !(e is XmlSchemaException) && !(e.InnerException is XmlException) && !(e.InnerException is XmlSchemaException);
        }

        #endregion

        #region

        /// <summary>
        ///     The get error count.
        /// </summary>
        /// <param name="errorCode">
        ///     The error code.
        /// </param>
        /// <returns>
        ///     The <see cref="long" />.
        /// </returns>
        private long GetErrorCount(string errorCode)
        {
            return errors[errorCode];
        }

        /// <summary>
        ///     The update error count.
        /// </summary>
        /// <param name="errorCode">
        ///     The error code.
        /// </param>
        private void UpdateErrorCount(string errorCode)
        {
            _totalErrors++;
            if (!errors.ContainsKey(errorCode))
                errors.Add(errorCode, 1);
            else
                errors[errorCode]++;
        }

        #endregion
    }
}