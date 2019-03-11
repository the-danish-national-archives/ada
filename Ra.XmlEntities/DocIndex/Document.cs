// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The document element from an EO1007 document index.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.DocIndex
{
    #region Namespace Using

    using System;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The document.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class Document
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the unique document id.
        /// </summary>
        [XmlElement("dID")]
        [XmlElement(DataType = "positiveInteger")]
        public string DocId { get; set; }

        /// <summary>
        ///     Gets or sets the document folder.
        /// </summary>
        [XmlElement("dCf")]
        public string DocumentFolder { get; set; }

        /// <summary>
        ///     Gets or sets the name of the schema file for GML documents.
        /// </summary>
        [XmlElement("gmlXsd")]
        public string GmlXsd { get; set; }

        /// <summary>
        ///     Gets or sets the media id.
        /// </summary>
        [XmlElement("mID")]
        [XmlElement(DataType = "positiveInteger")]
        public string MediaId { get; set; }

        /// <summary>
        ///     Gets or sets the original file name.
        /// </summary>
        [XmlElement("oFn")]
        [XmlElement(DataType = "normalizedString")]
        public string OriginalFileName { get; set; }

        /// <summary>
        ///     Gets or sets the parent document id for nested documents
        /// </summary>
        [XmlElement("pID")]
        [XmlElement(DataType = "positiveInteger", IsNullable = true)]
        public string ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the submission file type.
        /// </summary>
        [XmlElement("aFt")]
        public string SubmissionFileType { get; set; }

        #endregion
    }
}