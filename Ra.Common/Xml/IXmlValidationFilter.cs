// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXmlValidationFilter.cs" company="">
//   
// </copyright>
// <summary>
//   The XmlValidationFilter interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Ra.Common.Xml
{
    using System.IO;
    using System.Xml.Schema;

    /// <summary>
    ///     The XmlValidationFilter interface.
    /// </summary>
    public interface IXmlValidationFilter
    {
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
        XmlHandlerEventArgs ProcessEvent(ValidationEventArgs e, FileInfo sourceFileInfo);

        #endregion
    }
}