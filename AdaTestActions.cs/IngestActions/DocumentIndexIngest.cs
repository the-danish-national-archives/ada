namespace Ada.Common.IngestActions
{
    namespace Ada.ADA.Common.IngestActions
    {
        using Ra.Common;
        using Ra.Common.Xml;
        using Ra.DomainEntities.Documents;
        using System.Collections.Generic;
        using System.IO;
        using System.Xml.Linq;

        using global::Ada.Log;
        using global::Ada.Repositories;

        public class DocumentIndexIngest : AdaXmlIngest
        {
            public DocumentIndexIngest(IAdaProcessLog processLog,
                IAdaTestLog testLog,
                IArchivalXmlReader reader,
                IXmlEventLogger logger,
                AVMapping mapping)
                : base(processLog, testLog, reader, logger, mapping)
            {
            }

            protected override void OnRun(XmlCouplet targetXmlCouplet)
            {
                using (var repo = new DocumentIndexRepo(
                            new AdaTestUowFactory(
                            this.Mapping.AVID, "test",
                            new DirectoryInfo(Properties.Settings.Default.DBCreationFolder)),
                            1000))
                {
                    foreach (var doc in this.EnumerateDocIndexEntries(targetXmlCouplet))
                    {
                        repo.AddDocument(doc);
                    }
                    repo.Commit();
                }

                            
               (this.XmlLogger as XmlEventLogger).FlushLog();
            }

            public IEnumerable<DocIndexEntry> EnumerateDocIndexEntries(XmlCouplet targetXmlCouplet)
            {
                this.ArchivalXmlReader.Open(targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);
                foreach (var element in (this.ArchivalXmlReader as ArchivalXmlReader).ElementStream("doc"))
                {
                    XNamespace ns = "http://www.sa.dk/xmlns/diark/1.0";
                    var doc = new DocIndexEntry()
                                  {
                                      DocumentId = element.Element(ns + "dID")?.Value,
                                      ParentId =
                                          element.Element(ns + "pID")?.Attribute("{http://www.w3.org/2001/XMLSchema-instance}nil")  == null
                                              ? element.Element(ns + "pID")?.Value
                                              : null,
                                      MediaId = element.Element(ns + "mID")?.Value,
                                      DocumentFolder = element.Element(ns + "dCf")?.Value,
                                      OriginalFileName = element.Element(ns + "oFn")?.Value.Replace("\'", "''"),
                                      SubmissionFileType = element.Element(ns + "aFt")?.Value,
                                      GmlXsd = element.Element(ns + "gmlXsd")?.Value
                                  };

                    doc.Extension = this.getFileExtension(doc.OriginalFileName);

                    yield return doc;
                }
                this.ArchivalXmlReader.Close();
            }

            private string getFileExtension(string fileName)
            {
                var pos = fileName.LastIndexOf(".");
                if (pos >= 0)
                {
                    return fileName.Substring(pos);
                }
                return string.Empty;
            }
        }
    }
}