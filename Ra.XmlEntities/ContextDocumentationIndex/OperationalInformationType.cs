namespace Ra.XmlEntities.ContextDocumentationIndex
{
    #region Namespace Using

    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    #endregion

    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class OperationalInformationType
    {
        #region Properties

        /// <remarks />
        [XmlElement("operationalSystemInformation")]
        [DefaultValue(false)]
        public bool OperationalSystemInformation { get; set; }

        /// <remarks />
        [XmlElement("operationalSystemConvertedInformation")]
        [DefaultValue(false)]
        public bool OperationalSystemConvertedInformation { get; set; }


        /// <remarks />
        [XmlElement("operationalSystemSOA")]
        [DefaultValue(false)]
        public bool OperationalSystemSOA { get; set; }

        /// <remarks />
        [XmlElement("operationalSystemInformationOther")]
        [DefaultValue(false)]
        public bool OperationalSystemInformationOther { get; set; }

        #endregion
    }
}