namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    #endregion

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class ContextDocumentationIndexDocumentAuthor
    {
        #region Properties

        /// <remarks />
        [XmlElement("authorName")]
        public string AuthorName { get; set; }

        /// <remarks />
        [XmlElement("authorInstitution")]
        public string AuthorInstitution { get; set; }

        #endregion
    }
}