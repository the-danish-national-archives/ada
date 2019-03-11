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
    public class IngestInformationType
    {
        #region Properties

        /// <remarks />
        [XmlElement("archivistNotes")]
        [DefaultValue(false)]
        public bool ArchivistNotes { get; set; }

        /// <remarks />
        [XmlElement("archivalTestNotes")]
        [DefaultValue(false)]
        public bool ArchivalTestNotes { get; set; }

        /// <remarks />
        [XmlElement("archivalInformationOther")]
        [DefaultValue(false)]
        public bool ArchivalInformationOther { get; set; }

        #endregion
    }
}