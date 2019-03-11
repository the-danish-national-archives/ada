namespace Ra.XmlEntities.ArchiveIndex
{
    #region Namespace Using

    using System.Xml.Serialization;

    #endregion

    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class Form
    {
        #region Properties

        [XmlElement("formVersion")]
        public string FormVersion { get; set; }

        [XmlElement("classList")]
        public FormClass ClassList { get; set; }

        #endregion
    }
}