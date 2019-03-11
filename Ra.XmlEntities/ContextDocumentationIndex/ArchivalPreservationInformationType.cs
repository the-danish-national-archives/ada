namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.Xml.Serialization;

    #endregion

    [Serializable]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class ArchivalPreservationInformationType
    {
        #region Properties

        /// <remarks />
        [XmlElement("archivalMigrationInformation")]
        public bool ArchivalMigrationInformation { get; set; }

        /// <remarks />
        [XmlElement("archivalInformationOther")]
        public bool ArchivalInformationOther { get; set; }

        #endregion
    }
}