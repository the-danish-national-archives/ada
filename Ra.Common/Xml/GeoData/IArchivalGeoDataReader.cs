// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IArchivalGeoDataReader.cs" company="">
//   
// </copyright>
// <summary>
//   The ArchivalGeoDataReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml.GeoData
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Xml.Linq;

    #endregion

    /// <summary>
    ///     The ArchivalGeoDataReader interface.
    /// </summary>
    public interface IArchivalGeoDataReader
    {
        #region

        /// <summary>
        ///     Closes the reader.
        /// </summary>
        void Close();

        /// <summary>
        ///     Gives access to stream of Gml featureMembers
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<XElement> FeatureStream();

        /// <summary>
        ///     The geo data event. Reports EO1007-specific Gml  errors and warnings
        /// </summary>
        event GeoDataEventHandler GeoDataEvent;

        /// <summary>
        ///     Opens the Gml file. Reads and validates associated schemas, and the root of the Gml file itself
        /// </summary>
        /// <param name="stream">
        ///     The Gml stream.
        /// </param>
        /// <param name="schemaStream">
        ///     The Gml localschema stream.
        /// </param>
        void Open(BufferedProgressStream stream, BufferedProgressStream schemaStream);

        /// <summary>
        ///     Reads to the end of the Gml file
        /// </summary>
        void ReadToEnd();

        /// <summary>
        ///     The xml event. Reports xml errors and warnings
        /// </summary>
        event XmlEventHandler XmlEvent;

        #endregion
    }
}