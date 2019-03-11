namespace Ada.ADA.Common.IngestActions
{
    using System.IO;
    using Ada.Common;
    using Ada.Common.IngestActions;
    using Ada.Log;

    using Ra.Common;
    using Ra.Common.Xml.GeoData;

    public class GeoDataIngest: AdaActionBase<XmlCouplet>
    {

        protected readonly IArchivalGeoDataReader ArchivalXmlReader;

        protected readonly IXmlEventLogger XmlLogger;
        protected readonly BufferedProgressStream XmlStream;
        protected readonly BufferedProgressStream SchemaStream;

        protected readonly Stream LocalSchema;
        public GeoDataIngest(IAdaProcessLog processLog, IAdaTestLog testLog, IArchivalGeoDataReader reader, IXmlEventLogger logger, AVMapping mapping)         
            : base(processLog, testLog, mapping)
        {
            this.ArchivalXmlReader = reader;
            this.XmlLogger = logger;
            reader.XmlEvent += logger.EventHandler; 
        }

        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            this.ArchivalXmlReader.Open(targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);
            this.ArchivalXmlReader.ReadToEnd();
            this.ArchivalXmlReader.Close();           
        }
    }
}
