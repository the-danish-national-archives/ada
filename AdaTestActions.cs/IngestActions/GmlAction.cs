namespace Ada.Common.IngestActions
{
    using global::Ada.ADA.Common.IngestActions;
    using global::Ada.Core;
    using log4net;
    using Ra.Common;
    using Ra.Common.Repository;
    using Ra.Common.Xml;
    using Ra.Common.Xml.GeoData;
    using Ra.DomainEntities.Documents;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using global::Ada.ADA.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.DomainEntities;

    public class GmlAction : AdaActionBase<DocumentIndexRepo>
    {


        private readonly IAdaUowFactory testFactory;

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public GmlAction(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping) : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
        }

        protected override void OnRun(DocumentIndexRepo documentIndexRepo)
        {
            bool anXsdFileNeed = false;
                
                var gmlFiles = documentIndexRepo.EnumerateDocs(FileTypeEnum.Gml);
                
                foreach (var file in gmlFiles)
                {
                    if (file.GmlXsd != string.Empty) anXsdFileNeed = true;
                }


            if (anXsdFileNeed)
            {

                new AdaSingleQuery(this., base.TestLog, this.testFactory, this.Mapping).Run("5.G_1");
                new AdaSingleQuery(ProcessLog, TestLog, this.testFactory, this.Mapping).Run("5.G_3");



                var uowFactory = new AdaLogUowFactory(this.Mapping.AVID,"log", new DirectoryInfo(Properties.Settings.Default.DBCreationFolder));
                using (IUnitOfWork unitOfWork = uowFactory.GetUnitOfWork())
                {
                    var repository = unitOfWork.GetRepository<LogEntry>();

                    var query = repository.FilterBy(e => e.EntryTypeId == "5.G_1");
                    if (query.Any()) return;
                }
            }

            string localSharedPath = Path.Combine(this.Mapping.GetMediaRoot(1), this.Mapping.AVID.FullID + ".1",
                                                  "schemas",
                                                  "localShared");
            string standardPath = Path.Combine(this.Mapping.GetMediaRoot(1), this.Mapping.AVID.FullID + ".1",
                                               "schemas",
                                               "standard");




                var files = documentIndexRepo.EnumerateDocs(FileTypeEnum.Gml);

                foreach (var file in files)
                {
                    
                    var filter = new XmlEventFilter();
                    var streams = new List<BufferedProgressStream>();
                    var schemas = new List<string>
                                        {
                                            "basicTypes.xsd",
                                            "coordinateOperations.xsd",
                                            "coordinateReferenceSystems.xsd",
                                            "coordinateSystems.xsd",
                                            "coverage.xsd",
                                            "dataQuality.xsd",
                                            "datums.xsd",
                                            "defaultStyle.xsd",
                                            "dictionary.xsd",
                                            "direction.xsd",
                                            "dynamicFeature.xsd",
                                            "feature.xsd",
                                            "geometryAggregates.xsd",
                                            "geometryBasic2d.xsd",
                                            "geometryBasic0d1d.xsd",
                                            "geometryComplexes.xsd",
                                            "geometryPrimitives.xsd",
                                            "gml.xsd",
                                            "gmlBase.xsd",
                                            "grids.xsd",
                                            "measures.xsd",
                                            "observation.xsd",
                                            "referenceSystems.xsd",
                                            "smil20.xsd",
                                            "smil20-language.xsd",
                                            "temporal.xsd",
                                            "temporalReferenceSystems.xsd",
                                            "temporalTopology.xsd",
                                            "topology.xsd",
                                            "units.xsd",
                                            "valueObjects.xsd",
                                            "xlinks.xsd",
                                            "xml-mod.xsd"
                                        }; // TODO - read from db

                    foreach (string schema in schemas)
                    {
                        string path = Path.Combine(localSharedPath, schema);
                        streams.Add(new BufferedProgressStream(new FileInfo(path)));
                    }

                    string xmlxsdpath = Path.Combine(standardPath, "XMLSchema.xsd");
                    streams.Add(new BufferedProgressStream(new FileInfo(xmlxsdpath)));

                    ArchivalGeoDataReader reader = new ArchivalGeoDataReader(filter, streams);

                    Dictionary<XmlEventType, string> errorMap = new Dictionary<XmlEventType, string>();
                    errorMap.Add(XmlEventType.Exception, "0.2");
                    errorMap.Add(XmlEventType.XmlWellFormednessError, "5.G_17");
                    errorMap.Add(XmlEventType.XmlValidationError, "5.G_18");
                    errorMap.Add(XmlEventType.XmlValidationWarning, "5.G_18");
                    errorMap.Add(XmlEventType.SchemaValidationError, "5.G_4");
                    errorMap.Add(XmlEventType.SchemaWellFormedNessError, "5.G_5");
                    errorMap.Add(XmlEventType.XmlDeclaredEncodingIllegal, "5.G_20");
                    errorMap.Add(XmlEventType.XmlMissingProlog, "5.G_22");
                    errorMap.Add(XmlEventType.SchemaDeclaredEncodingIllegal, "5.G_6");

                    IXmlEventLogger logger = new XmlEventLogger(this.ReportLogEntry, errorMap, this.Mapping);
                    GeoDataEventLogger schemalogger = new GeoDataEventLogger(this.ReportLogEntry);
                    reader.GeoDataEvent += schemalogger.GeoDataEventHandler;

                    string folderPath = Path.Combine(this.Mapping.GetMediaPath(Int32.Parse(file.MediaId)),
                        "Documents", file.DocumentFolder, file.DocumentId);
                        
                    string gmlPath = Path.Combine(folderPath, "1.gml");
                    string xsdPath = Path.Combine(file.GmlXsd == "1.xsd" ? folderPath : localSharedPath, file.GmlXsd);

                    var xsdFileInfo = new FileInfo(xsdPath);
                    var gmlFileInfo = new FileInfo(gmlPath);

                    Regex RxValidXsdName = new Regex(@"^localschema\d+.xsd$|\d+.xsd$");
                    var match = RxValidXsdName.Match(file.GmlXsd.ToLower());

                    if (!match.Success)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "4.C.6_2" };
                        logEntry.AddTag("Path", gmlPath);
                        this.ReportLogEntry(logEntry);
                        continue;
                    }

                    if (!xsdFileInfo.Exists)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "5.G_2" };
                        logEntry.AddTag("Name", xsdFileInfo.FullName);
                        this.ReportLogEntry(logEntry);
                        continue;
                    }

             

                    if (!gmlFileInfo.Exists)
                    {
                        continue; // caught in structure test
                    }

                    if (gmlFileInfo.Length >= 1000000000)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "5.G_16" };
                        logEntry.AddTag("Path", gmlFileInfo.FullName);
                        this.ReportLogEntry(logEntry);
                        continue;
                    }

                    var gmlStream = new BufferedProgressStream(gmlFileInfo);
                    var xsdStream = new BufferedProgressStream(xsdFileInfo);
                    new GeoDataIngest(ProcessLog, this.TestLog, reader, logger, this.Mapping).Run(new XmlCouplet(gmlStream, xsdStream));
               
            }
            }


    }
}