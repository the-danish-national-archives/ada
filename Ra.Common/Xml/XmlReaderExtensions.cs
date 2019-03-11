// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlReaderExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The xml reader extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Xsl;

    #endregion

    /// <summary>
    ///     The xml reader extensions.
    /// </summary>
    public static class XmlReaderExtensions
    {
        #region

        public static List<XAttribute> ReadAttributes(this XmlReader reader)
        {
            var attributelist = new List<XAttribute>();
            if (reader.HasAttributes)
                for (var i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    var ns = reader.LookupNamespace(reader.Prefix);
                    if (ns != null) attributelist.Add(new XAttribute(XNamespace.Get(ns) + reader.LocalName, reader.Value));
                }

            return attributelist;
        }

        /// <summary>
        ///     The transform.
        /// </summary>
        /// <param name="reader">
        ///     The reader.
        /// </param>
        /// <param name="transformXsl">
        ///     The transform xsl.
        /// </param>
        /// <returns>
        ///     The <see cref="XmlReader" />.
        /// </returns>
        public static XmlReader Transform(this XmlReader reader, string transformXsl)
        {
            var sr = new StringReader(transformXsl);
            var xslReader = XmlReader.Create(sr);

            var xslt = new XslCompiledTransform();
            xslt.Load(xslReader);

            var stream = new MemoryStream();
            xslt.Transform(reader, null, stream);
            stream.Position = 0;

            return XmlReader.Create(stream);
        }

        #endregion
    }
}