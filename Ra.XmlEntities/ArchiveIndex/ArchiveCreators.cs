namespace Ra.XmlEntities.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "archiveCreatorList")]
    public class ArchiveCreators
    {
        #region Properties

        [XmlElement(ElementName = "creatorName")]
        public List<string> CreatorName { get; set; }

        [XmlElement(ElementName = "creationPeriodStart")]
        public List<string> CreationPeriodStart { get; set; }

        [XmlElement(ElementName = "creationPeriodEnd")]
        public List<string> CreationPeriodEnd { get; set; }

        #endregion
    }
}