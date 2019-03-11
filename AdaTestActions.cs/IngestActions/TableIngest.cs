namespace Ada.ADA.Common.TestActions
{
    namespace Ada.ADA.Common.TestActions
    {
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Linq;

        using global::Ada.Common;
        using global::Ada.Common.IngestActions;
        using global::Ada.Log;
        using global::Ada.Repositories;

        using Ra.Common;
        using Ra.Common.Repository.NHibernate;
        using Ra.Common.Xml;
        using Ra.DomainEntities.FileIndex;
        using Ra.DomainEntities.FileIndex.Extensions;
        using Ra.DomainEntities.TableIndex;
        using Ra.EntityExtensions.TableIndex;

        public class TableIngest: AdaActionBase<IAdaUowFactory>
        {        
            protected override void OnRun(IAdaUowFactory factory)
            {
                var avUowFactory = new AdaAvUowFactory(this.Mapping.AVID,"av" ,new DirectoryInfo(Properties.Settings.Default.DBCreationFolder));

                using (var uow = (UnitOfWork)factory.GetUnitOfWork())
                {
                    foreach (var table in uow.GetRepository<Table>().All().ToList())
                    {
                        GC.Collect(1);
                        var insertTemplate = table.GetInsertTemplate();

                        var errorMap = new Dictionary<XmlEventType, string>
                                            {
                                                { XmlEventType.Exception, "0.2" },
                                                { XmlEventType.XmlWellFormednessError,"4.D_8" },
                                                { XmlEventType.XmlValidationError,"4.D_9" },
                                                { XmlEventType.XmlValidationWarning,"4.D_9" },
                                                { XmlEventType.XmlMissingProlog, "4.D_13" },
                                                { XmlEventType.XmlDeclaredEncodingIllegal, "4.D_14" }
                                            };

                        IXmlEventLogger logger = new XmlEventLogger(this.ReportLogEntry, errorMap, this.Mapping);
                        var reader = new ArchivalXmlReader(new XmlEventFilter());
                        reader.XmlEvent += logger.EventHandler;

                        var fileRepo = uow.GetRepository<FileIndexEntry>();
                        var tableMetaData = fileRepo.All().FirstOrDefault(x => x.Extension == ".xml" && x.FileName == table.Folder);
                        if (tableMetaData == null)
                            continue;

                        var path = 
                            this.Mapping.GetMediaPath(Int32.Parse(tableMetaData.MediaNumber)) + 
                            tableMetaData.RelativePathAndFile();

                        var tableXmlStream = new BufferedProgressStream(new FileInfo(path));
                        var tableXsdStream = new BufferedProgressStream(table.GetXmlSchemaStream());

                        using (var tableRepo = new TableContentRepo(avUowFactory, 1000))
                        {
                            try
                            {

                                reader.Open(tableXmlStream, tableXsdStream);
                                foreach (var rowElement in reader.ElementStream("row"))
                                {
                                    var rowContents = new SortedList<string, string>();
                                    foreach (var columnElement in rowElement.Elements())
                                    {
                                        rowContents.Add(columnElement.Name.LocalName, columnElement.Value.Replace("\'", "''"));
                                    }
                                    tableRepo.AddRow(insertTemplate, rowContents);
                                }
                                reader.Close();
                                tableRepo.Commit();

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }              
            }

            public TableIngest(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
                : base(processLog, testLog, mapping)
            {
            }
        }
    }
}