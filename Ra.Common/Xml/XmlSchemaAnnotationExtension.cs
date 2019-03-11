namespace Ra.Common.Xml
{
    #region Namespace Using

    using System.Linq;
    using System.Xml.Schema;

    #endregion

    public static class XmlSchemaAnnotationExtension
    {
        #region

        public static bool IsEmpty(this XmlSchemaAnnotation annotation)
        {
            if (annotation != null)
            {
                var documentationList = annotation.Items.OfType<XmlSchemaDocumentation>();
                if (
                    documentationList.Any(
                        documentation => documentation.Markup.ToList().Exists(x => !string.IsNullOrEmpty(x.InnerText))))
                    return false;
            }

            return true;
        }

        #endregion
    }
}