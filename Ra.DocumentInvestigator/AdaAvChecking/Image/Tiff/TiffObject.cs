namespace Ra.DocumentInvestigator.AdaAvChecking.Image.Tiff
{
    #region Namespace Using

    using System;
    using System.Linq;
    using System.Reflection;
    using AdaReporting;
    using Common;
    using filetypes;
    using log4net;
    using Leadtools;

    #endregion

    public class TiffObject : BaseDocObject, ISubPages
    {
        #region  Fields

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region  Constructors

        public TiffObject
            (BufferedProgressStream currentStream, DocInfo docInfo, Action<DocLogEntry> callback = null, int settings = (int) TiffChecks.ALL, int? highPageCount = null)
            : base(currentStream, docInfo, callback, IsOnlyMetaDataNeeded((TiffChecks) settings))
        {
            Settings = (TiffChecks) settings;
            log.Debug("ctor");
            IsTiff = FileType == LegalFileType.Tif;
            if (!IsTiff)
            {
                var logEvent = new DocLogEntryE12(DocInfo);
                Callback(logEvent);
                return;
            }


            try
            {
                currentStream.Buffer();
                if ((Settings & TiffChecks.PRIVATE_TAGS) != 0)
                    RasterCodecs.Options.Load.Tags = true;

                var pageCount = PageCount; // load PageCount

                if (highPageCount != null && PageCount > highPageCount)
                    Callback(new DocLogEntryE13(DocInfo, PageCount));

                PageObjects = new PageObject[PageCount];

                LoadInformation();
                ParseInformation();
            }
            catch (Exception ex) when (
                ex is TiffPageReadException
                || ex is TiffReadException
                || ex is RasterException)
            {
                log.Error(ex);
                Callback(new DocLogEntryE2(DocInfo));
            }
        }

        #endregion

        #region Properties

        public bool IsAllPagesBalnk => PageObjects.All(p => p.BlankDokumentPage ?? true);

        public bool IsAnyOddDBI => PageObjects.Where(tiffPageInfo => tiffPageInfo != null)
            .Any(tiffPageInfo => tiffPageInfo.OddDBI);

        public bool IsCompressionLegal => TestIfCompressionIsLegal() == CompressionEnum.Legal;

        public bool IsFillorderLegal => PageObjects.Where(p => p != null)
            .All(p => p.Fillorder == FillorderEnum.FillorderLegal);

        public bool IsFirstPageBlank
            => PageObjects.First().BlankDokumentPage ?? false;


        public bool IsPrivateTagsFound => PageObjects.Where(tiffPageInfo => tiffPageInfo != null)
            .Any(tiffPageInfo => tiffPageInfo.PrivateTagsInPage);


        public bool IsTiff { get; }
        private TiffChecks Settings { get; }

        #endregion

        #region ISubPages Members

        public PageObject[] PageObjects { get; }

        #endregion

        #region

        private static bool IsOnlyMetaDataNeeded(TiffChecks settings)
        {
            return
                (settings & (TiffChecks.BLANK_FIRST_PAGE | TiffChecks.PRIVATE_TAGS | TiffChecks.ALL_PAGES_BLANK)) == 0;
        }

        private void LoadInformation()
        {
            if (PageCount <= 0)
                return;

            for (var pageNo = 1; pageNo <= PageCount; pageNo++)
            {
                EnsurePageNumber(pageNo);
                PageObjects[pageNo - 1] = new PageObject
                {
                    PageNo = pageNo,
                    BlankDokumentPage = Settings.HasFlag(TiffChecks.BLANK_FIRST_PAGE) && pageNo == 1
                                        || Settings.HasFlag(TiffChecks.ALL_PAGES_BLANK)
                        ? TestBlankPage(pageNo)
                        : null,
                    PrivateTagsInPage = Settings.HasFlag(TiffChecks.PRIVATE_TAGS) && PrivateTagsTest(pageNo),
                    OddDBI = TestIfDPIIsOdd(),
                    Compression = GetPageCompression(),
                    Fillorder = TestTifFillorderLegal()
                };
            }
        }

        public sealed override void ParseInformation()
        {
            if (Settings.HasFlag(TiffChecks.BLANK_FIRST_PAGE)
                && IsFirstPageBlank)
                Callback(new DocLogEntryE10(DocInfo));

            if (Settings.HasFlag(TiffChecks.ALL_PAGES_BLANK)
                && IsAllPagesBalnk)
                Callback(new DocLogEntryE11(DocInfo));


            if (Settings.HasFlag(TiffChecks.PRIVATE_TAGS)
                && IsPrivateTagsFound)
                Callback(new DocLogEntryE7(DocInfo));


            if (Settings.HasFlag(TiffChecks.COMPRESSION)
                && PageObjects.Where(p => p != null)
                    .FirstOrDefault(p => !VerifyCompressionByType(p)) is PageObject pageObject)
                Callback(new DocLogEntryE3(DocInfo, pageObject));


            if (IsAnyOddDBI)
                Callback(new DocLogEntryE5(DocInfo));

            if (!IsTiff) // Unreachable code, this is caught already in start of contructor
                Callback(new DocLogEntryE10(DocInfo));

            if (!IsFillorderLegal)
                Callback(new DocLogEntryE2(DocInfo));
        }

        private CompressionEnum TestIfCompressionIsLegal()
        {
            if (PageCount == 0)
                return CompressionEnum.Error; //"-1,Ingen sider eller anden fejl";

            foreach (var item in PageObjects)
                if (!VerifyCompressionByType(item))
                    return CompressionEnum.Illegal;
            return CompressionEnum.Legal;
        }

        // Tests fill order for current page
        private FillorderEnum TestTifFillorderLegal()
        {
            //"CCITT GROUP 4 FAX" (tif) 
            //"LZW" (tif)
            //"CCITT GROUP 3 FAX" (tif)
            //"RUN LENGTH" (tif PackBit)
            //"NONE" (ukomprimeret tif)
            //"JPEG" (tif)
            //"JBIG" (tif)
            //"JPEG2000 LOSSY" (j2k) 

            const int FillOrderTag = 266;


            if (ImageInfoFormat != RasterImageFormat.TifPackBits && ImageInfoFormat != RasterImageFormat.TifLzw)
                return FillorderEnum.FillorderLegal;


            return
                string.CompareOrdinal(GetDocumentTiffTag(FillOrderTag), "2") == 0
                    ? FillorderEnum.FillorderIllegal
                    : FillorderEnum.FillorderLegal;

            //Indlæs sideinfo
//            if (((info.Compression.ToUpper() == "RUN LENGTH") || (info.Compression.ToUpper() == "LZW"))
//                && (this.GetDocumentTiffTag(page.PageNo, FillOrderTag) == "2"))
        }

        #endregion
    }

    [Flags]
    public enum TiffChecks
    {
        NONE = 0,

        COMPRESSION = 1 << 0,

        PRIVATE_TAGS = 1 << 1,

        BLANK_FIRST_PAGE = 1 << 2,

        ALL_PAGES_BLANK = 1 << 3,

        ALL = COMPRESSION | PRIVATE_TAGS | BLANK_FIRST_PAGE | ALL_PAGES_BLANK
    }
}