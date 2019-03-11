// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXmlEventFilter.cs" company="">
//   
// </copyright>
// <summary>
//   The XmlValidationFilter interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System;

    #endregion

    /// <summary>
    ///     The XmlValidationFilter interface.
    /// </summary>
    public interface IXmlEventFilter
    {
        #region

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
        XmlHandlerEventArgs ProcessEventArgs(XmlHandlerEventArgs e);

        bool ShouldRethrow(Exception e);

        #endregion
    }
}