namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using ActionBase;
    using Common;
    using Log;
    using Ra.Common.Xml.GeoData;

    #endregion

    [AdaActionPrecondition("files")]
    public class GeoDataIngestAction : AdaActionBase<XmlCouplet>
    {
        #region  Fields

        protected readonly IArchivalGeoDataReader ArchivalXmlReader;

        protected readonly IXmlEventLogger XmlLogger;

        #endregion

        #region  Constructors

        public GeoDataIngestAction(IAdaProcessLog processLog, IAdaTestLog testLog, IArchivalGeoDataReader reader, IXmlEventLogger logger, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            ArchivalXmlReader = reader;
            XmlLogger = logger;
            reader.XmlEvent += logger.EventHandler;
        }

        #endregion

        #region

        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            ArchivalXmlReader.Open(targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);
            ArchivalXmlReader.ReadToEnd();
            ArchivalXmlReader.Close();
        }

        #endregion
    }
}