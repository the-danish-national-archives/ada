// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArchivalXmlReader.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The ArchivalXmlReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System;

    #endregion

    /// <summary>
    ///     The ArchivalXmlReader interface.
    /// </summary>
    public interface IArchivalXmlReader
    {
        #region

        /// <summary>
        ///     The close.
        /// </summary>
        void Close();

        /// <summary>
        ///     The deserialize.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        object Deserialize(Type type);

        /// <summary>
        ///     The open.
        /// </summary>
        /// <param name="transformXsl">
        ///     The transform xsl.
        /// </param>
        void Open(BufferedProgressStream stream, BufferedProgressStream schemastreams = null);

        void ReadToEnd();

        event XmlEventHandler XmlEvent;

        #endregion
    }
}