namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using ActionBase;
    using Common;
    using Log;
    using Ra.Common;
    using Ra.Common.Xml;
    using Repositories;

    #endregion

    public abstract class AdaXmlIngestAction<TRepository, TEntity> : AdaIngestAction<TEntity, TRepository, XmlCouplet> where TRepository : IAdaIngestRepository<TEntity>
    {
        #region  Fields

        protected readonly IArchivalXmlReader ArchivalXmlReader;

        #endregion

        #region  Constructors

        protected AdaXmlIngestAction
        (IAdaProcessLog processLog, IAdaTestLog testLog,
//            IArchivalXmlReader reader,
            IXmlEventLogger logger, AVMapping mapping, TRepository targetRepository)
            : base(processLog, testLog, mapping, targetRepository)
        {
            Logger = logger;
            ArchivalXmlReader = new ArchivalXmlReader(new XmlEventFilter()); // reader;
            //            this.XmlLogger = GetLogger(testLog, mapping);
            ArchivalXmlReader.XmlEvent += logger.EventHandler;
        }

        #endregion

        #region Properties

        public IXmlEventLogger Logger { get; }

        #endregion

//        protected readonly IXmlEventLogger XmlLogger;

        //{
        //    //foreach (var entity in loader.Load(this.ArchivalXmlReader, couplet.XmlStream, couplet.SchemaStream))

        //    //    this.TargetRepository.SaveEntity(entity);
        //}
    }


    //public abstract class AdaXmlIngestFunction<TRepository> : AdaIngestAction<XmlCouplet, TRepository> where TRepository : IAdaRepository
    //{
    //    protected AdaXmlIngestFunction(IAdaProcessLog processLog, IAdaTestLog testLog, IArchivalXmlReader reader, IXmlEventLogger logger, AVMapping mapping, TRepository targetRepository)
    //        : base(processLog, testLog, mapping, targetRepository)
    //    {
    //        this.ArchivalXmlReader = reader;
    //        this.XmlLogger = logger;
    //        reader.XmlEvent += logger.EventHandler;
    //    }

    //    protected readonly IArchivalXmlReader ArchivalXmlReader;
    //    protected readonly IXmlEventLogger XmlLogger;
    //}

    public class XmlCouplet
    {
        #region  Fields

        private readonly string _idName;

        #endregion

        #region  Constructors

        public XmlCouplet(BufferedProgressStream xmlStream, BufferedProgressStream schemaStream, string idName) : this(idName)
        {
            SchemaStream = schemaStream;
            XmlStream = xmlStream;
        }

        public XmlCouplet(string idName)
        {
            _idName = idName;
        }

        #endregion

        #region Properties

        public BufferedProgressStream SchemaStream { get; }

        public BufferedProgressStream XmlStream { get; }

        #endregion

        #region

        public override string ToString()
        {
            return _idName;
        }

        #endregion
    }
}