namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    #endregion

    [Serializable]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class SubmissionInformationType
    {
        #region Properties

        /// <remarks />
        [XmlElement("archivalProvisions")]
        [DefaultValue(false)]
        public bool ArchivalProvisions { get; set; }

        /// <remarks />
        [XmlElement("archivalTransformationInformation")]
        [DefaultValue(false)]
        public bool ArchivalTransformationInformation { get; set; }

        /// <remarks />
        [XmlElement("archivalInformationOther")]
        [DefaultValue(false)]
        public bool ArchivalInformationOther { get; set; }

        #endregion
    }
}