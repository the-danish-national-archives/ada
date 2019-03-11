namespace Ra.XmlEntities.ArchiveIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false)]
    public class archiveIndex
    {
        #region Properties

        [XmlElement(ElementName = "archiveType")]
        public bool ArchiveType { get; set; }

        [XmlElement(ElementName = "SystemName")]
        public string SystemName { get; set; }

        [XmlElement("alternativeName")]
        public string[] AlternativeName { get; set; }

        [XmlElement(ElementName = "multipleDataCollection")]
        public bool MultipleDataCollection { get; set; }

        [XmlElement(ElementName = "personalDataRestrictedInfo")]
        public bool PersonalDataRestrictedInfo { get; set; }

        [XmlElement(ElementName = "otherAccessTypeRestrictions")]
        public bool OtherAccessTypeRestrictions { get; set; }

        [XmlElement(ElementName = "archiveApproval")]
        public string ArchiveApproval { get; set; }

        [XmlElement(ElementName = "archiveInformationPackageIDPrevious")]
        public string ArchiveInformationPackageIDPrevious { get; set; }

        [XmlElement(ElementName = "archivePeriodStart")]
        public string ArchivePeriodStart { get; set; }

        [XmlElement(ElementName = "archivePeriodEnd")]
        public string ArchivePeriodEnd { get; set; }

        [XmlElement(ElementName = "archiveInformationPacketType")]
        public bool ArchiveInformationPacketType { get; set; }

        [XmlElement(ElementName = "archiveCreatorList")]
        public ArchiveCreators ArchiveCreators { get; set; }

        [XmlElement(ElementName = "archiveInformationPackageID")]
        public string ArchiveInformationPackageID { get; set; }


        [XmlElement(ElementName = "archiveRestrictions")]
        public string ArchiveRestrictions { get; set; }


        [XmlElement(ElementName = "cvrNum")]
        public bool CvrNum { get; set; }

        [XmlElement(ElementName = "matrikNum")]
        public bool MatrikNum { get; set; }

        [XmlElement(ElementName = "bbrNum")]
        public bool BbrNum { get; set; }

        [XmlElement("sourceName")]
        public List<string> SourceName { get; set; }

        [XmlElement("userName")]
        public List<string> UserName { get; set; }

        [XmlElement("predecessorName")]
        public string[] PredecessorName { get; set; }

        [XmlElement(ElementName = "form")]
        public Form Form { get; set; }

        [XmlElement(ElementName = "containsDigitalDocuments")]
        public bool ContainsDigitalDocuments { get; set; }

        [XmlElement(ElementName = "systemPurpose")]
        public string SystemPurpose { get; set; }

        [XmlElement(ElementName = "systemContent")]
        public string SystemContent { get; set; }

        [XmlElement(ElementName = "regionNum")]
        public bool RegionNum { get; set; }

        [XmlElement(ElementName = "komNum")]
        public bool KomNum { get; set; }

        [XmlElement(ElementName = "cprNum")]
        public bool CprNum { get; set; }


        [XmlElement("relatedRecordsName")]
        public List<string> RelatedRecordsName { get; set; }

        [XmlElement(ElementName = "systemFileConcept")]
        public bool SystemFileConcept { get; set; }


        [XmlElement(ElementName = "searchRelatedOtherRecords")]
        public bool SearchRelatedOtherRecords { get; set; }


        [XmlElement(ElementName = "whoSygKod")]
        public bool WhoSygKod { get; set; }

        #endregion
    }
}