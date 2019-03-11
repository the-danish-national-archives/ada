namespace Ra.XmlEntities.ArchiveIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "classList")]
    public class FormClass
    {
        #region Properties

        [XmlElement("formClass")]
        public List<string> Class { get; set; }

        [XmlElement("formClassText")]
        public List<string> ClassText { get; set; }

        #endregion
    }
}