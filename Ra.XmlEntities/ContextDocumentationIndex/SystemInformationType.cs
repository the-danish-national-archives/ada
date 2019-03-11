namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    #endregion

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class SystemInformationType
    {
        #region Properties

        /// <remarks />
        [XmlElement("systemPurpose")]
        public bool SystemPurpose { get; set; }

        /// <remarks />
        [XmlElement("systemRegulations")]
        public bool SystemRegulations { get; set; }

        /// <remarks />
        [XmlElement("systemContent")]
        public bool SystemContent { get; set; }

        /// <remarks />
        [XmlElement("systemAdministrativeFunctions")]
        public bool SystemAdministrativeFunctions { get; set; }

        /// <remarks />
        [XmlElement("systemDataTransfer")]
        public bool SystemDataTransfer { get; set; }

        /// <remarks />
        [XmlElement("systemPreviousSubsequentFunctions")]
        public bool SystemPreviousSubsequentFunctions { get; set; }

        /// <remarks />
        [XmlElement("systemAgencyQualityControl")]
        public bool SystemAgencyQualityControl { get; set; }


        /// <remarks />
        [XmlElement("systemPresentationStructure")]
        public bool SystemPresentationStructure { get; set; }

        /// <remarks />
        [XmlElement("systemDataProvision")]
        public bool SystemDataProvision { get; set; }


        /// <remarks />
        [XmlElement("systemPublication")]
        public bool SystemPublication { get; set; }

        /// <remarks />
        [XmlElement("systemInformationOther")]
        public bool SystemInformationOther { get; set; }

        #endregion
    }
}