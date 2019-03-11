using Ra.Common.Xml;

namespace Ada.Common.IngestActions
{
    using global::Ada.ActionBase;
    using global::Ada.ADA.Common;
    using global::Ada.Log;

    using Ra.Common;
    using Ra.Common.Repository;

    public abstract class AdaXmlIngest : AdaActionBase<XmlCouplet>
    {
        protected AdaXmlIngest(IAdaProcessLog processLog, IAdaTestLog testLog, IArchivalXmlReader reader, IXmlEventLogger logger, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.ArchivalXmlReader = reader;
            this.XmlLogger = logger;        
            reader.XmlEvent += logger.EventHandler;
        }

        protected readonly IUnitOfWork uow;
        protected readonly IArchivalXmlReader ArchivalXmlReader;
        protected readonly IXmlEventLogger XmlLogger;
    }

    public class XmlCouplet
    {
        public XmlCouplet(BufferedProgressStream xmlStream, BufferedProgressStream schemaStream)
        {
            this.SchemaStream = schemaStream;
            this.XmlStream = xmlStream;
        }

        public BufferedProgressStream XmlStream { get; private set; }

        public BufferedProgressStream SchemaStream { get; private set; }
    }

}