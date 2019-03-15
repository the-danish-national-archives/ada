namespace Ra.DocumentInvestigator.AdaAvChecking.Image
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Reflection;
    using System.Threading;
    using System.Timers;
    using AdaReporting;
    using Common;
    using filetypes;
    using log4net;
    using Leadtools;
    using Leadtools.Codecs;
    using Leadtools.ImageProcessing.Color;
    using LEADToolsHelper;
    using Tiff;
    using Timer = System.Timers.Timer;

    #endregion

    public abstract class BaseDocObject : IDisposable //, IDocObject
    {
        #region Static

        private static readonly HashSet<string> ValidCompressions = new HashSet<string>(
            new[]
            {
                RasterImageFormat.TifLzw, //"LZW":
                RasterImageFormat.TifLzwCmyk,
                RasterImageFormat.CcittGroup31Dim, // "CCITT GROUP 3 FAX"
                RasterImageFormat.CcittGroup4, //"CCITT GROUP 4 FAX"
                RasterImageFormat.TifPackBits, // "RUN LENGTH"
                RasterImageFormat.TifPackBitsCmyk
            }.Select(f => f.ToString()));

        private static Timer _timer;
        private static Thread RasterThread;

        #endregion

        #region  Fields

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        protected readonly Action<DocLogEntry> Callback;

        private readonly bool MetaDataOnly;

        protected readonly TypeDetector typeDetector;

        private int _currentPage = 1;
        private CodecsImageInfo _imageInfo;


        private RasterCollection<RasterTagMetadata> _imageTags;

        /// <summary>
        ///     MetaData for image container
        /// </summary>
        private CodecsImageInfo _infoAll;

        private uint? _pageCount;

        private RasterCodecs _rasterCodecs;
  
        private RasterImage _rasterImg;

        public int RasterNo = 1;

        private RasterImage RrasterImg;

        public double TimerSeconds = 6000;

        #endregion

        #region  Constructors

        static BaseDocObject()
        {
            LEADToolsUnlocked.EnsureUnlocked();
        }


        public BaseDocObject
        (BufferedProgressStream currentStream, DocInfo docInfo, Action<DocLogEntry> callback = null,
            bool metaDataOnly = false)
        {
            MetaDataOnly = metaDataOnly;
            //            this._log.Debug("ctor");
            typeDetector = new TypeDetector(currentStream);

            _currentStream = currentStream;

            DocInfo = docInfo;

            FileFullName = currentStream.file.FullName;

            Name = Path.Combine(DocInfo.DocumentFolder, DocInfo.DocumentId, currentStream.file?.Name ?? "");

            Callback = callback ?? (e => { });
        }

        #endregion

        #region Properties

        protected Stream _currentStream { get; private set; }

        //        protected readonly BufferedProgressStream _currentStream;
        protected Stream CurrentStreamFromZero
        {
            get
            {
                _currentStream.Position = 0;
                return _currentStream;
            }
        }

        protected DocInfo DocInfo { get; set; }


        protected string FileFullName { get; }

        public LegalFileType FileType
            => typeDetector.VerifyFiletypeByMagicNumberBasedOnExtension(FileFullName);

        /// <summary>
        ///     MetaData for each page
        /// </summary>
        protected CodecsImageInfo ImageInfo
        {
            get
            {
                if (_imageInfo == null)
                    try
                    {
                        _imageInfo = RasterCodecs.GetInformation(
                            //                                                   FileFullName
                            CurrentStreamFromZero
                            , false, _currentPage);
                    }
                    catch (Exception e)
                    {
                        throw new TiffPageReadException(_currentPage, Name, e);
                    }

                return _imageInfo;
            }
        }


        //        protected RasterImageFormat ImageInfoFormat => ImageInfo.Format;
        protected RasterImageFormat ImageInfoFormat =>
            MetaDataOnly ? ImageInfo.Format : RasterImage.OriginalFormat;

        private RasterCollection<RasterTagMetadata>
            ImageTags
        {
            get
            {
                Debug.Assert(MetaDataOnly == false);
                if (!MetaDataOnly)
                    return RasterImage.Tags;

                if (_imageTags == null)
                    try
                    {
                        _imageTags = RasterCodecs.ReadTags(
                            //                                                                     FileFullName
                            CurrentStreamFromZero
                            , _currentPage);
                    }
                    catch (Exception e)
                    {
                        throw new TiffPageReadException(_currentPage, Name, e);
                    }

                return _imageTags;
            }
        }

        public static bool JustPassedTreshold { get; set; }


        public string Name { get; }

        // TODO A lot happens when pagecount is called (a wrapper steam might be introduced), find a better way when a property read to trigger that
        public uint PageCount => _pageCount ??
                                 (uint) (_pageCount = ReadPageCount());

        protected RasterCodecs RasterCodecs => _rasterCodecs ?? (_rasterCodecs = InitializeLeadTools());

        protected RasterImage RasterImage
        {
            get
            {
                Debug.Assert(MetaDataOnly == false,
                    "Full page requested to be loaded even though Metadata only was set.");

                if (_rasterImg == null)
                {
                    void RasterRead()
                    {
                        try
                        {
                            _rasterImg = RasterCodecs.Load(
                                //FileFullName
                                CurrentStreamFromZero
                                , _currentPage);
                            RrasterImg = _rasterImg;
                        }
                        catch (Exception e)
                        {
                            
                        }
                        finally
                        {
                            StopPeriodicTimer();
                        }
                    }

                    ThreadStart threadStart = RasterRead;

                    RasterThread = new Thread(threadStart);
                    RasterThread.Name = "RasterThread" + RasterNo;
                    RasterNo++;
                    RasterThread.Start();
                    RasterThread.Join();
                    if (_rasterImg == null)
                        throw new TiffPageReadException(_currentPage, Name, new Exception("Document '" + FileFullName +
                                                                                          "' used too much memory when it tried to load."));

                    return RrasterImg;
                }


                return _rasterImg;
            }
        }


        public static bool SecondTresholdPassed { get; set; }


        public static bool ThirdTresholdPassed { get; set; }


        public static bool TresholdPassed { get; set; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
            GC.ReRegisterForFinalize(this);
        }

        #endregion

        #region

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _rasterImg?.Dispose();
                _rasterImg = null;
                _rasterCodecs?.Dispose();
                _rasterCodecs = null;
            }
        }

        protected void EnsurePageNumber(int sideNr)
        {
            // Todo start loading this and/or next page via Task
            if (_currentPage != sideNr)
            {
                _rasterImg?.Dispose();
                _rasterImg = null;
                _imageTags = null;
                _imageInfo?.Dispose();
                _imageInfo = null;
                _currentPage = sideNr;
                //                Debug.WriteLine($"Set to page {sideNr} in {_currentStream.file.FullName}");
            }
        }


        protected string GetDocumentTiffTag(int sideNr, int id)
        {
            EnsurePageNumber(sideNr);
            return GetDocumentTiffTag(id);
        }

        protected string GetDocumentTiffTag(int id)
        {
            //Byte      = 1  -  8-bit unsigned integer
            //Ascii     = 2  -  8-bit bytes w/ last byte null
            //Short     = 3  - 16-bit unsigned integer
            //Long      = 4  - 32-bit unsigned integer
            //Rational  = 5  - 64-bit unsigned fraction
            //SByte     = 6  -  8-bit signed integer
            //Undefined = 7  -  8-bit untyped data (byte)
            //SShort    = 8  - 16-bit signed integer
            //SLong     = 9  - 32-bit signed integer
            //SRational = 10 - 64-bit signed fraction
            //Float     = 11 - 32-bit IEEE floating point
            //Double    = 12 - 64-bit IEEE floating point
            //Ifd       = 13 - 32-bit unsigned integer (offset)

            RasterTagMetadata tag;

            if (RasterCodecs.Options.Load.Tags)
                tag = ImageTags.FirstOrDefault(t => t.Id == id);
            else
                tag
                    = RasterCodecs.ReadTag(
                        CurrentStreamFromZero,
                        //                FileFullName, 
                        _currentPage, id);


            var tagDataStr = "";
            //Tjek om der er data ...
            if (tag != null) tagDataStr = TagDataToString(tag);

            return tagDataStr;
        }

        protected string GetPageCompression()
        {
            return ImageInfoFormat.ToString();
        }



        protected RasterCodecs InitializeLeadTools()
        {
            // initialize the codecs object.
            try
            {
                return new RasterCodecs();
            }
            catch (RasterException ex)
            {
                _log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Test om bestemt side er blank. Returnerer true eller false.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pageNr"></param>
        /// <returns></returns>
        private static bool IsPageBlank(RasterImage img)
        {
            //  RasterImage rasterImg;

            var col = img.GetPixelColor(0, 0).ToRgb();
            //int yMid = img.Height / 2;
            var xMid = img.Width / 2;

            //Vertikal test testes først og fanger flertallet
            for (var y = 0; y < img.Height; y++)
                if (img.GetPixelColor(y, xMid).ToRgb() != col)
                {
                    StopPeriodicTimer();

                    //    if (img.GetPixelColor(xMid, y).ToRgb() != col)
                    return false;
                }

            //Hvis der er indikationer på at side er blank testes siden intensivt 
            var colorCount = 0;

            // Prepare the command 
            var command = new ColorCountCommand();

            //Tæl antallet af anvendte farver på side 
            command.Run(img);
            colorCount = command.ColorCount;

            StopPeriodicTimer();
            return colorCount == 1;
        }


        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

                var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new
                {
                    FreePhysicalMemory = double.Parse(mo["FreePhysicalMemory"].ToString()),
                    TotalVisibleMemorySize = double.Parse(mo["TotalVisibleMemorySize"].ToString())
                }).FirstOrDefault();
                var Treshold = 60;

                if (memoryValues != null)
                {
                    var percent = (memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) /
                                  memoryValues.TotalVisibleMemorySize * 100;
#if DEBUG
                    Console.WriteLine("Memory used on current document:" + percent + "%");
#endif


                    if (!TresholdPassed)
                        if (percent > Treshold)
                        {
                            if (!TresholdPassed)
                                JustPassedTreshold = true;
                            TresholdPassed = true;
                            RasterThread.Abort();
                            StopPeriodicTimer();
                            
                        }

                    if (!SecondTresholdPassed)
                    {
                        Treshold = 75;
                        if (percent > Treshold)
                        {
                            if (!SecondTresholdPassed)
                                JustPassedTreshold = true;

                            SecondTresholdPassed = true;
                            RasterThread.Abort();
                            StopPeriodicTimer();
                            
                        }
                    }
                    else
                    {
                        Treshold = 85;
                        if (percent > Treshold)
                        {
                            if (!ThirdTresholdPassed)
                                JustPassedTreshold = true;
                            ThirdTresholdPassed = true;
                            RasterThread.Abort();
                            StopPeriodicTimer();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public abstract void ParseInformation();

        private void PeriodicChecksOfMemory()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Name = "TimerThread";
                SetPeriodicTimer();
            }).Start();
        }


        protected bool PrivateTagsTest(int sideNr)
        {
            const int copyRight = 33432; //33432 "CopyRight" er en del af baseline 6.0 standarden

            Debug.Assert(MetaDataOnly == false);

            EnsurePageNumber(sideNr);


            //            return found;
            return ImageTags.Any(t => t.Id > copyRight);
        }

        private uint ReadPageCount()
        {
            try
            {
                _currentStream.Seek(0, SeekOrigin.Begin);
                var count = RasterCodecs.GetTotalPages(_currentStream);
                if (count < 1)
                    throw new Exception();


                if (count > 20 && !(_currentStream is SimpleCachedStream))
                    _currentStream = new SimpleCachedStream(_currentStream);

                return (uint) count;
            }
            catch (Exception e)
            {
                throw new TiffReadException(FileFullName, e);
            }
        }

        private void SetPeriodicTimer()
        {
            // Create a timer with an interval.
            if (_timer == null)
            {
                _timer = new Timer(TimerSeconds);
                // Hook up the Elapsed event for the timer. 
                _timer.Elapsed += OnTimedEvent;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
        }


        private static void StopPeriodicTimer()
        {
            // Stop a timer.
            if (_timer != null && false) _timer.Stop();
        }

        /// <summary>
        ///     Læs Tag data og konverter til string
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="tagDataStr"></param>
        /// <returns></returns>
        private static string TagDataToString(RasterTagMetadata tag)
        {
            var tagDataStr = string.Empty;
            var readTagData = tag.GetData();

            switch (tag.DataType)
            {
                case RasterTagMetadataDataType.Ascii:
                    tagDataStr = tag.ToAscii();
                    break;
                case RasterTagMetadataDataType.Single:
                    tagDataStr = BitConverter.ToSingle(readTagData, 0).ToString(CultureInfo.CurrentCulture);
                    break;
                case RasterTagMetadataDataType.Double:
                    tagDataStr = BitConverter.ToDouble(readTagData, 0).ToString(CultureInfo.CurrentCulture);
                    break;
                case RasterTagMetadataDataType.UInt16:
                case RasterTagMetadataDataType.Int16:
                    tagDataStr = BitConverter.ToUInt16(readTagData, 0).ToString();
                    break;
                case RasterTagMetadataDataType.UInt32:
                case RasterTagMetadataDataType.Int32:
                    tagDataStr = BitConverter.ToUInt32(readTagData, 0).ToString();
                    break;
                case RasterTagMetadataDataType.URational:
                case RasterTagMetadataDataType.Rational:
                    tagDataStr = BitConverter.ToUInt32(readTagData, 0) + "/" + BitConverter.ToInt32(readTagData, 4);
                    break;
                case RasterTagMetadataDataType.Undefined:
                    tagDataStr = BitConverter.ToString(readTagData, 0);
                    break;
            }

            return tagDataStr;
        }

        protected bool? TestBlankPage(int i)
        {
            try
            {
                EnsurePageNumber(i);
     
                PeriodicChecksOfMemory();


                return IsPageBlank(RasterImage);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    if (e.InnerException.Message.Contains("too much memory"))
                        throw;
                _log.Error(e);
                return null; //Fejl - uspecificeret
            }
        }

        protected bool TestIfDPIIsOdd()
        {
            if (MetaDataOnly)
            {
                if (ImageInfo.XResolution == ImageInfo.YResolution) return false; //Alt OK
            }
            else
            {
                if (RasterImage.XResolution == RasterImage.YResolution) return false; //Alt OK
            }

            switch (ImageInfoFormat)
            {
                case RasterImageFormat.TifxFaxG31D:
                case RasterImageFormat.TifxFaxG32D:
                case RasterImageFormat.TifxFaxG4:
                    return false; //Alt OK
                default:
                    return true;
            }
        }

        protected static bool VerifyCompressionByType(PageObject item)
        {
            return ValidCompressions.Contains(item.Compression);

        }

        #endregion
    }
}
