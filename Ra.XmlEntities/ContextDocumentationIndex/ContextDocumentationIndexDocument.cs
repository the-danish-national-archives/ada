// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextDocumentationIndexDocument.cs" company="´Rigsarkivet">
//   
// </copyright>
// <summary>
//   The context documentation document element from an EO1007 context documentation index.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The context documentation document.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "contextDocumentationIndexDocument")]
    public class ContextDocumentationDocument
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the authors.
        /// </summary>
        [XmlElement("documentAuthor")]
        public List<ContextDocumentationIndexDocumentAuthor> Authors { get; set; }

        /// <summary>
        ///     Gets or sets the document id.
        /// </summary>
        [XmlElement(ElementName = "documentID")]
        public string DocumentId { get; set; }

        /// <summary>
        ///     Gets or sets the document title.
        /// </summary>
        [XmlElement(ElementName = "documentTitle")]
        public string DocumentTitle { get; set; }

        /// <summary>
        ///     Gets or sets the document category.
        /// </summary>
        [XmlElement(ElementName = "documentCategory")]
        public DocumentCategoryType DocumentCategory { get; set; }

        /// <summary>
        ///     Gets or sets the document date.
        /// </summary>
        [XmlElement(ElementName = "documentDate")]
        public string DocumentDate { get; set; }

        /// <summary>
        ///     Gets or sets the document description.
        /// </summary>
        [XmlElement(ElementName = "documentDescription")]
        public string DocumentDescription { get; set; }

        #endregion
    }
}