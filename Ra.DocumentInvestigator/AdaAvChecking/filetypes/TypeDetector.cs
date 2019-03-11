namespace Ra.DocumentInvestigator.AdaAvChecking.filetypes
{
    #region Namespace Using

    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using audioVideo;
    using Common;
    using log4net;

    #endregion

    public class TypeDetector
    {
        #region  Fields

        private readonly BufferedProgressStream _stream;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MediaProcessor mediator = new MediaProcessor();

        #endregion

        #region  Constructors

        public TypeDetector(BufferedProgressStream stream = null)
        {
            _stream = stream;
        }

        #endregion

        #region

        //*************************************************************  
        //********************  Hjælpefunktioner  *********************
        //*************************************************************

        /// <summary>
        ///     Returnerer Extension som LowerCase uden adskilletegn fx "tif"
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileExtension(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            if (ext[0].ToString() == ".") return ext.Remove(0, 1); //Fjern dot
            return ext;
            //Fjern dot
        }

        /// <summary>
        ///     Returnerer Extension som LowerCase uden adskilletegn fx "tif"
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            var filename = Path.GetFileName(filePath).ToLower();
            if (filename.Length > 0) return filename; //Fjern dot
            return "";
            //Fjern dot
        }

//        /// <summary>
//        ///     Hurtig test om indhold af fil svarer til type angivet i extension, men ikke om compression/codec etc. er ok
//        /// </summary>
//        /// <param name="filnavn"></param>
//        /// <returns></returns>
//        public bool TestFileTypeAgainstExtension(string filnavn)
//        {
//            var ext = GetFileExtension(filnavn);
//
//            switch (ext)
//            {
//                case "tif":
//                    return this.TestTifByMagicNumberOld(filnavn);
//                case "jp2":
//                    return TestJP2ByMagicNumberOld(filnavn);
//
//                case "mpg":
//                case "wav":
//                case "mp3":
//                    return mediator.TestMediaStreamByImageInfo(filnavn) == ImageInfoResult.SomeStream;
//                case "gml":
//                    return this.TestXML_ByFirstMarkupChar(filnavn);
//                case "xsd":
//                    return this.TestXML_ByFirstMarkupChar(filnavn);
//
//                default:
//                    return false; //Hvis der finde en fil med ukendt ext hvilket ikke bør kunne ske? 
//            }
//        }

        public bool IsJp2(string filnavn)
        {
            //  00 00 00 0C 6A 50 20 20 0D 0A
            return VerifyFiletypeByMagicNumberBasedOnExtension(filnavn) == LegalFileType.Jp2;
        }


        /// <summary>
        ///     Simpel typetest som læser 50 bytes og tester om første reelle karakter er et mindre end tegn
        /// </summary>
        /// <param name="filnavn"></param>
        /// <returns></returns>
        public bool TestXML_ByFirstMarkupChar(string filnavn)
        {
            var chars = new char[50];
            var sr = new StreamReader(filnavn, Encoding.UTF8);
            var read = sr.ReadBlock(chars, 0, 50);
            sr.Close();
            var str = new string(chars, 0, read).Trim(); //
            return str[0] == '<';
        }


        /// <summary>
        ///     Verifies the filetype by magic number based on extension.
        ///     A tif file recognized so first on its file extension and
        ///     are then tested for whether it is a tiffil using magic numbers -
        ///     if it is returns LegalFileType.Tif otherwise LegalFileType.Unknown
        /// </summary>
        /// <param name="filnavn">The filnavn.</param>
        /// <returns>The filetype - unless its not a valid filetype (either by unknown or incorrekt extension)</returns>
        public LegalFileType VerifyFiletypeByMagicNumberBasedOnExtension(string filnavn)
        {
            var ext = GetFileExtension(filnavn);

            var stream = _stream;

            try
            {
                if (stream == null)
                    stream = new BufferedProgressStream(new FileInfo(filnavn));


                switch (ext)
                {
                    case "tif":
                        // if(this.TestTifByMagicNumberOld(filnavn))return LegalFileType.Tif;
                        if (VerifyTIFFByMagicNumber(stream)) return LegalFileType.Tif;
                        break;
                    case "jp2":
                        if (VerifyJ2PByMagicNumber(stream)) return LegalFileType.Jp2;
                        break;
                    case "wav":
                        if (VerifyWAVByMagicNumber(stream)) return LegalFileType.Wav;
                        break;
                    case "mpg":
                    case "mp3":
                        //TODO udvides med et korrekt test for magic numbers
                        if (mediator.TestMediaStreamByImageInfo(filnavn) == ImageInfoResult.SomeStream)
                            return LegalFileType.MP3;
                        break;
                    case "gml":
                        if (TestXML_ByFirstMarkupChar(filnavn)) return LegalFileType.GML;
                        //TODO Hvordan tester man forskellen på en XSD og GML?
                        break;
                    case "xsd":
                        if (VerifyXMLByMagicNumber(stream)) return LegalFileType.XSD;
                        break;
                    default:
                        return LegalFileType.Unknown;
                }
            }
            finally
            {
                if (_stream == null)
                    stream?.Dispose();
            }

            return LegalFileType.Unknown;
        }


        /// <summary>
        ///     Verifies the filetype by magic numbers.
        /// </summary>
        /// <param name="tiffStream">The tiff stream.</param>
        /// <param name="magics">The magics.</param>
        /// <returns></returns>
        private bool VerifyFiletypeByMagicNumbers(BufferedProgressStream tiffStream, byte[][] magics)
        {
            var bytesNeeded = magics.Max(i => i.Length);

            tiffStream.Position = 0;
            var size = bytesNeeded;
            var bytes = new byte[size];
            tiffStream.Read(bytes, 0, bytes.Length);

            return magics.Any(item => bytes.Take(item.Length).SequenceEqual(item));
        }

        private bool VerifyJ2PByMagicNumber(BufferedProgressStream stream)
        {
            var magics = new[] {ConstantArrayOfMagicNumbers.JP2};
            return VerifyFiletypeByMagicNumbers(stream, magics);
        }

        /// <summary>
        ///     Simpel typetest som læser 23 bytes og tester op mod JP2 magic number
        /// </summary>
        /// <param name="filnavn"></param>
        /// <returns></returns>
//        public bool TestJP2ByMagicNumberOld(string filnavn)
//        {
//            //00 00 00 0C 6A 50 20 20 0D 0A 87 0A 00 00 00 14 66 74 79 70 6A 70 32
//            //jpeg2000 Code Stream = FF 4F FF 51 00 00 (jpc og j2k)
//            var bytes = new byte[15];
//
//            try
//            {
//                var s = new FileStream(filnavn, FileMode.Open, FileAccess.Read);
//                s.Read(bytes, 0, 15);
//                s.Close();
//            }
//            catch
//            {
//                this.log.Error("Fejl i forbindelse med filtypetest af: " + filnavn);
//                return false;
//            }
//
//
//            var h = new HexEncoding();
//            var str = h.ToString(bytes);
//
//            if (str == "0000000C6A5020200D0A870A000000") //jpeg2000 file format (jp2)
//            {
//                return true;
//            }
//            return false;
//        }

        /// <summary>
        ///     Verifies the file is a mp3 by magic number.
        /// </summary>
        /// <param name="tiffStream">The tiff stream.</param>
        /// <returns>true if it is</returns>
        public bool VerifyMP3ByMagicNumber(BufferedProgressStream tiffStream)
        {
            var magics = new[] {ConstantArrayOfMagicNumbers.Mp3WithID3V2Container, ConstantArrayOfMagicNumbers.Mp3WithoutID3};
            return VerifyFiletypeByMagicNumbers(tiffStream, magics);
        }

        /// <summary>
        ///     Verifies the file is tiff by magic number.
        /// </summary>
        /// <param name="tiffStream">The tiff stream.</param>
        /// <returns></returns>
        internal bool VerifyTIFFByMagicNumber(BufferedProgressStream tiffStream)
        {
            var magics = new[] {ConstantArrayOfMagicNumbers.TiffBigEndian, ConstantArrayOfMagicNumbers.TiffLittleEndian};
            return VerifyFiletypeByMagicNumbers(tiffStream, magics);
        }

        internal bool VerifyWAVByMagicNumber(BufferedProgressStream tiffStream)
        {
            var magics = new[] {ConstantArrayOfMagicNumbers.WAVRiff, ConstantArrayOfMagicNumbers.WAVWave};
            return VerifyFiletypeByMagicNumbers(tiffStream, magics);
        }

        internal bool VerifyXMLByMagicNumber(BufferedProgressStream tiffStream)
        {
            var magics = new[] {ConstantArrayOfMagicNumbers.XMLTwoBytes, ConstantArrayOfMagicNumbers.XMLFourBytes};
            return VerifyFiletypeByMagicNumbers(tiffStream, magics);
        }

        #endregion
    }
}