namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    #endregion

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class DocumentCategoryType
    {
        #region Properties

        [XmlElement("operationalInformation")]
        public OperationalInformationType OperationalInformation { get; set; }

        [XmlElement("submissionInformation")]
        public SubmissionInformationType SubmissionInformation { get; set; }

        [XmlElement("ingestInformation")]
        public IngestInformationType IngestInformation { get; set; }

        [XmlElement("archivalPreservationInformation")]
        public ArchivalPreservationInformationType ArchivalPreservationInformation { get; set; }

        [XmlElement("informationOther")]
        public InformationOtherType InformationOther { get; set; }


        [XmlElement("systemInformation")]
        public SystemInformationType SystemInformation { get; set; }

        #endregion
    }
}