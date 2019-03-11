namespace Ada.Common.IngestActions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;

    using global::Ada.Log;
    using global::Ada.Repositories;

    using Ra.Common;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Xml;
    using Ra.DomainEntities;
    using Ra.DomainEntities.FileIndex;

    public class FileIndexIngestAction : XmlIngestAction
    {
        public FileIndexIngestAction(IAdaProcessLog processLog, IAdaTestLog testLog,
            IArchivalXmlReader reader,
            IXmlEventLogger logger, AVMapping mapping)
            : base(processLog, testLog, reader, logger, mapping)
        {          
        }

        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            using (var repo = new AdaFileIndexRepo(
                            new AdaTestUowFactory(
                            this.Mapping.AVID, "test",
                            new DirectoryInfo(Properties.Settings.Default.DBCreationFolder)),
                            1000))
            {
                foreach (var file in this.EnumerateFileIndexEntries(targetXmlCouplet))
                {
                    repo.AddFile(file);
                }
                repo.Commit();
            }    
        }

        public IEnumerable<FileIndexEntry> EnumerateFileIndexEntries(XmlCouplet targetXmlCouplet)
        {
            this.ArchivalXmlReader.Open(targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);
            foreach (var element in (this.ArchivalXmlReader as ArchivalXmlReader).ElementStream("f"))
            {
                XNamespace ns = "http://www.sa.dk/xmlns/diark/1.0";

                var folderName = element.Element(ns + "foN")?.Value;
                var root = folderName.GetRootFolder();

                var file = new FileIndexEntry
                {
                    MediaNumber = AViD.ExtractMediaNumber(root),
                    RelativePath = folderName?.Replace(root, string.Empty),
                    FileName = Path.GetFileNameWithoutExtension(element.Element(ns + "fiN")?.Value),
                    Extension = Path.GetExtension(element.Element(ns + "fiN")?.Value),
                    Md5 = element.Element(ns + "md5")?.Value,
                    TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
         
                };
                yield return file;
            }   
            this.ArchivalXmlReader.Close();                               
        } 
    }
}