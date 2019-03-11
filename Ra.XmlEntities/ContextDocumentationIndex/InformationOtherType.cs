namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.Xml.Serialization;

    #endregion

    [Serializable]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class InformationOtherType
    {
        #region Properties

        /// <remarks />
        [XmlElement("informationOther")]
        public bool InformationOther { get; set; }

        #endregion
    }
}