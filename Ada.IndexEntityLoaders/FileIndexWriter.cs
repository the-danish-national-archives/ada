namespace Ada.EntityLoaders
{
    #region Namespace Using

    using System.IO;
    using System.Xml;
    using Ra.DomainEntities;
    using Ra.DomainEntities.FileIndex;

    #endregion

    public class FileIndexWriter
    {
        #region  Fields

        private XmlWriter writer;

        #endregion

        #region

        public void Close()
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        public void Open(Stream stream)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            writer = XmlWriter.Create(stream, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("fileIndex", "http://www.sa.dk/xmlns/diark/1.0");
            writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sa.dk/xmlns/diark/1.0 ../Schemas/standard/fileIndex.xsd");
        }

        public void WriteElement(FileIndexEntry file, AViD aviD)
        {
            writer.WriteStartElement("f");
//            this.writer.WriteElementString("foN", string.Concat(aviD.FullID, ".", file.MediaNumber, file.RelativePath));
            writer.WriteElementString("foN", file.foN);
            writer.WriteElementString("fiN", string.Concat(file.FileName, file.Extension));
            writer.WriteElementString("md5", file.Md5);
            writer.WriteEndElement();
            writer.Flush();
        }

        #endregion
    }
}