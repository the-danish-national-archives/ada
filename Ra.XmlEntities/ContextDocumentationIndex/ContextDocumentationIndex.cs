namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false,
        ElementName = "contextDocumentationIndex")]
    public class ContextDocumentationIndex
    {
        #region Properties

        [XmlElement("document")]
        public List<ContextDocumentationDocument> Documents { get; set; }

        #endregion
    }
}