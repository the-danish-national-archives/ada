namespace Ra.DocumentInvestigator.OldForWinforms
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using log4net;
    using Leadtools;
    using Leadtools.Codecs;
    using LEADToolsHelper;

    #endregion

    public class TiffLeadToolsProcessor
    {
        #region Static

        private static Dictionary<int, TifTag> tifTagsDictionary;

        private static Dictionary<string, string> compressionNamesDictionary;

        #endregion

        #region  Fields

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private RasterCodecs codecs;

        #endregion

        #region  Constructors

        public TiffLeadToolsProcessor()
        {
            InitializeLeadTools();
            LoadTifTagDictionary();
            LoadCompressionNames();
            codecs = new RasterCodecs();
        }

        #endregion

        #region

        /// <summary>
        ///     Returnerer formateret Tag information for side i TIFF dokument
        /// </summary>
        /// <param name="filnavn"></param>
        /// <param name="sideNr"></param>
        /// <returns></returns>
        public List<TifTag> GetDocumentTiffTags(string filnavn, int sideNr)
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


            var resultTagList = new List<TifTag>();

            var tagDataStr = "";
            RasterCollection<RasterTagMetadata> tagList;
            try
            {
                tagList = codecs.ReadTags(filnavn, sideNr);
            }
            catch
            {
                return resultTagList;
            }

            foreach (var tag in tagList)
            {
                //Læs Tag data og konverter til string
                var readTagData = tag.GetData();

                if (tag.DataType == RasterTagMetadataDataType.Ascii)
                    tagDataStr = tag.ToAscii();
                else if (tag.DataType == RasterTagMetadataDataType.Single)
                    tagDataStr = BitConverter.ToSingle(readTagData, 0).ToString();
                else if (tag.DataType == RasterTagMetadataDataType.Double)
                    tagDataStr = BitConverter.ToDouble(readTagData, 0).ToString();
                else if (tag.DataType == RasterTagMetadataDataType.UInt16
                         || tag.DataType == RasterTagMetadataDataType.Int16)
                    tagDataStr = BitConverter.ToUInt16(readTagData, 0).ToString();
                else if (tag.DataType == RasterTagMetadataDataType.UInt32
                         || tag.DataType == RasterTagMetadataDataType.Int32)
                    tagDataStr = BitConverter.ToUInt32(readTagData, 0).ToString();
                else if (tag.DataType == RasterTagMetadataDataType.URational
                         || tag.DataType == RasterTagMetadataDataType.Rational)
                    tagDataStr = BitConverter.ToUInt32(readTagData, 0) + "/"
                                                                       + BitConverter.ToInt32(readTagData, 4);
                else if (tag.DataType == RasterTagMetadataDataType.Undefined)
                    tagDataStr = BitConverter.ToString(readTagData, 0); // Tag.ToAscii();

                //Hvis tag er 259 (kompression), oversættes tagDataStr fra nummer til sigende kompressions navn
                if (tag.Id == 259)
                {
                    var tempStr = GetTiffKompressionFriendlyName(tagDataStr);
                    if (tempStr == "")
                        tagDataStr = "Ukendt (" + tagDataStr + ")";
                    else
                        tagDataStr = tempStr;
                }

                //Data lægges i struct
                var tagInf = new TifTag
                {
                    tagNo = tag.Id,
                    data = tagDataStr
                };

                //Hent yderligere tag information
                tagInf = GetTiffTagInfo(tagInf);

                //Opsaml tags
                resultTagList.Add(tagInf);
            }

            //Leadtools.Codecs.RasterCodecs.Shutdown();

            return resultTagList;
        }

        /// <summary>
        ///     Returnerer antal sider for et dokument
        /// </summary>
        /// <param name="fil"></param>
        /// <returns></returns>
        public int GetPageCount(string fil)
        {
            try
            {
                const int AdobeDummy = 99999; //Dummyværdi som sikrer læsning af sideantal fra Adobe Photoshop MP filer
                var info = codecs.GetInformation(fil, true, AdobeDummy);
                return info.TotalPages;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        ///     Oversætter Tif kompressionskode fx "4" til forståeligt navn fx "CCITT Group 4 FAX"
        /// </summary>
        /// <param name="tagData"></param>
        /// <returns></returns>
        private static string GetTiffKompressionFriendlyName(string tagData)
        {
            //"CCITT GROUP 4 FAX" (tif) 
            //"LZW" (tif)
            //"CCITT GROUP 3 FAX" (tif)
            //"RUN LENGTH" (tif PackBit)
            //"NONE" (ukomprimeret tif)
            //"JPEG" (tif)
            //"JBIG" (tif)
            //"JPEG2000 LOSSY" (j2k)

            string myValue;
            return compressionNamesDictionary.TryGetValue(tagData, out myValue) ? myValue : "";
        }

        private static TifTag GetTiffTagInfo(TifTag tag)
        {
            TifTag myValue;

            if (!tifTagsDictionary.TryGetValue(tag.tagNo, out myValue)) return new TifTag {name = "", gruppe = "", hint = ""};

            myValue.tagNo = tag.tagNo;
            myValue.data = tag.data;
            return myValue;
        }

        private void InitializeLeadTools()
        {
            // initialize the codecs object.
            try
            {
                LEADToolsUnlocked.EnsureUnlocked();
                codecs = new RasterCodecs();
            }
            catch (RasterException ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        private static void LoadCompressionNames()
        {
            compressionNamesDictionary = new Dictionary<string, string>
            {
                {"1", "UnCompressed"},
                {"2", "CCITT 1D (Huffman RLE)"},
                {"3", "CCITT Group 3 FAX"},
                {"4", "CCITT Group 4 FAX"},
                {"5", "LZW"},
                {"6", "JPG (lossy)"},
                {"7", "JPEG"},
                {"8", "Deflate (Adobe)"},
                {"9", "JBIG (ITU-T Rec. T.85)"},
                {"10", "JBIG (ITU-T Rec. T.43)"},
                {"32766", "NeXT 2-bit encoding"},
                {"32771", "CCITT RLE with word alignment"},
                {"32773", "PackBit"},
                {"32809", "ThunderScan 4-bit encoding"},
                {"32895", "IT8 CT w/padding"},
                {"32896", "IT8 Linework RLE"},
                {"32897", "IT8 Monochrome picture"},
                {"32898", "IT8 Binary line art"},
                {"32908", "PIXARFILM companded 10 bit LZW"},
                {"32909", "PIXAR companded 11 bit ZIP"},
                {"32910", "PIXAR"},
                {"32911", "PIXAR"},
                {
                    "32946",
                    "PKZIP-style Deflate encoding (experimental)"
                },
                {
                    "32947",
                    "Kodak DCS encoding (Oceana Matrix)"
                },
                {"34661", "JBIG (IS3O)"},
                {
                    "34676",
                    "SGI 32-bit Log Luminance encoding (experimental)"
                },
                {
                    "34677",
                    "SGI 24-bit Log Packed Luminance encoding (experimental)"
                },
                {"34712", "JP2000"}
            };
        }

        private static void LoadTifTagDictionary()
        {
            tifTagsDictionary = new Dictionary<int, TifTag>
            {
                {
                    254, new TifTag
                    {
                        name = "NewSubfileType",
                        gruppe = "Baseline",
                        hint =
                            "A general indication of the kind of data contained in this subfile."
                    }
                },
                {
                    255, new TifTag
                    {
                        name = "SubfileType",
                        gruppe = "Baseline",
                        hint =
                            "A general indication of the kind of data contained in this subfile."
                    }
                },
                {
                    256, new TifTag
                    {
                        name = "ImageWidth",
                        gruppe = "Baseline",
                        hint =
                            "The number of columns in the image, i.e., the number of pixels per row."
                    }
                },
                {
                    257, new TifTag
                    {
                        name = "ImageLength",
                        gruppe = "Baseline",
                        hint = "The number of rows of pixels in the image."
                    }
                },
                {
                    258,
                    new TifTag
                    {
                        name = "BitsPerSample",
                        gruppe = "Baseline",
                        hint = "Number of bits per component."
                    }
                },
                {
                    259, new TifTag
                    {
                        name = "Compression",
                        gruppe = "Baseline",
                        hint = "Compression scheme used on the image data."
                    }
                },
                {
                    262, new TifTag
                    {
                        name = "PhotometricInterpretation",
                        gruppe = "Baseline",
                        hint = "The color space of the image data."
                    }
                },
                {
                    263, new TifTag
                    {
                        name = "Threshholding",
                        gruppe = "Baseline",
                        hint =
                            "For black and white TIFF files that represent shades of gray, the technique used to convert from gray to black and white pixels."
                    }
                },
                {
                    264, new TifTag
                    {
                        name = "CellWidth",
                        gruppe = "Baseline",
                        hint =
                            "The width of the dithering or halftoning matrix used to create a dithered or halftoned bilevel file."
                    }
                },
                {
                    265, new TifTag
                    {
                        name = "CellLength",
                        gruppe = "Baseline",
                        hint =
                            "The length of the dithering or halftoning matrix used to create a dithered or halftoned bilevel file."
                    }
                },
                {
                    266, new TifTag
                    {
                        name = "FillOrder",
                        gruppe = "Baseline",
                        hint = "The logical order of bits within a byte."
                    }
                },
                {
                    270, new TifTag
                    {
                        name = "ImageDescription",
                        gruppe = "Baseline",
                        hint = "A string that describes the subject of the image."
                    }
                },
                {
                    271,
                    new TifTag
                    {
                        name = "Make",
                        gruppe = "Baseline",
                        hint = "The scanner manufacturer."
                    }
                },
                {
                    272,
                    new TifTag
                    {
                        name = "Model",
                        gruppe = "Baseline",
                        hint = "The scanner model name or number."
                    }
                },
                {
                    273, new TifTag
                    {
                        name = "StripOffsets",
                        gruppe = "Baseline",
                        hint = "For each strip, the byte offset of that strip."
                    }
                },
                {
                    274, new TifTag
                    {
                        name = "Orientation",
                        gruppe = "Baseline",
                        hint =
                            "The orientation of the image with respect to the rows and columns."
                    }
                },
                {
                    277, new TifTag
                    {
                        name = "SamplesPerPixel",
                        gruppe = "Baseline",
                        hint = "The number of components per pixel."
                    }
                },
                {
                    278,
                    new TifTag
                    {
                        name = "RowsPerStrip",
                        gruppe = "Baseline",
                        hint = "The number of rows per strip."
                    }
                },
                {
                    279, new TifTag
                    {
                        name = "StripByteCounts",
                        gruppe = "Baseline",
                        hint =
                            "For each strip, the number of bytes in the strip after compression."
                    }
                },
                {
                    280,
                    new TifTag
                    {
                        name = "MinSampleValue",
                        gruppe = "Baseline",
                        hint = "The minimum component value used."
                    }
                },
                {
                    281,
                    new TifTag
                    {
                        name = "MaxSampleValue",
                        gruppe = "Baseline",
                        hint = "The maximum component value used."
                    }
                },
                {
                    282, new TifTag
                    {
                        name = "XResolution",
                        gruppe = "Baseline",
                        hint =
                            "The number of pixels per ResolutionUnit in the ImageWidth direction."
                    }
                },
                {
                    283, new TifTag
                    {
                        name = "YResolution",
                        gruppe = "Baseline",
                        hint =
                            "The number of pixels per ResolutionUnit in the ImageLength direction."
                    }
                },
                {
                    284, new TifTag
                    {
                        name = "PlanarConfiguration",
                        gruppe = "Baseline",
                        hint = "How the components of each pixel are stored."
                    }
                },
                {
                    288, new TifTag
                    {
                        name = "FreeOffsets",
                        gruppe = "Baseline",
                        hint =
                            "For each string of contiguous unused bytes in a TIFF file, the byte offset of the string."
                    }
                },
                {
                    289, new TifTag
                    {
                        name = "FreeByteCounts",
                        gruppe = "Baseline",
                        hint =
                            "For each string of contiguous unused bytes in a TIFF file, the number of bytes in the string."
                    }
                },
                {
                    290, new TifTag
                    {
                        name = "GrayResponseUnit",
                        gruppe = "Baseline",
                        hint =
                            "The precision of the information contained in the GrayResponseCurve."
                    }
                },
                {
                    291, new TifTag
                    {
                        name = "GrayResponseCurve",
                        gruppe = "Baseline",
                        hint =
                            "For grayscale data, the optical density of each possible pixel value."
                    }
                },
                {
                    296, new TifTag
                    {
                        name = "ResolutionUnit",
                        gruppe = "Baseline",
                        hint =
                            "The unit of measurement for XResolution and YResolution."
                    }
                },
                {
                    305, new TifTag
                    {
                        name = "Software",
                        gruppe = "Baseline",
                        hint =
                            "Name and version number of the software package(s) used to create the image."
                    }
                },
                {
                    306,
                    new TifTag
                    {
                        name = "DateTime",
                        gruppe = "Baseline",
                        hint = "Date and time of image creation."
                    }
                },
                {
                    315,
                    new TifTag
                    {
                        name = "Artist",
                        gruppe = "Baseline",
                        hint = "Person who created the image."
                    }
                },
                {
                    316, new TifTag
                    {
                        name = "HostComputer",
                        gruppe = "Baseline",
                        hint =
                            "The computer and/or operating system in use at the time of image creation."
                    }
                },
                {
                    320,
                    new TifTag
                    {
                        name = "ColorMap",
                        gruppe = "Baseline",
                        hint = "A color map for palette color images."
                    }
                },
                {
                    338,
                    new TifTag
                    {
                        name = "ExtraSamples",
                        gruppe = "Baseline",
                        hint = "Description of extra components."
                    }
                },
                {
                    33432,
                    new TifTag
                    {
                        name = "Copyright",
                        gruppe = "Baseline",
                        hint = "Copyright notice."
                    }
                },
                {
                    269, new TifTag
                    {
                        name = "DocumentName",
                        gruppe = "Extension",
                        hint =
                            "The name of the document from which this image was scanned."
                    }
                },
                {
                    285, new TifTag
                    {
                        name = "PageName",
                        gruppe = "Extension",
                        hint =
                            "The name of the page from which this image was scanned."
                    }
                },
                {
                    286,
                    new TifTag
                    {
                        name = "XPosition",
                        gruppe = "Extension",
                        hint = "X position of the image."
                    }
                },
                {
                    87,
                    new TifTag
                    {
                        name = "YPosition",
                        gruppe = "Extension",
                        hint = "Y position of the image."
                    }
                },
                {
                    292,
                    new TifTag
                    {
                        name = "T4Options",
                        gruppe = "Extension",
                        hint = "Options for Group 3 Fax compression"
                    }
                },
                {
                    293,
                    new TifTag
                    {
                        name = "T6Options",
                        gruppe = "Extension",
                        hint = "Options for Group 4 Fax compression"
                    }
                },
                {
                    297, new TifTag
                    {
                        name = "PageNumber",
                        gruppe = "Extension",
                        hint =
                            "The page number of the page from which this image was scanned."
                    }
                },
                {
                    301, new TifTag
                    {
                        name = "TransferFunction",
                        gruppe = "Extension",
                        hint =
                            "Describes a transfer function for the image in tabular style."
                    }
                },
                {
                    317, new TifTag
                    {
                        name = "Predictor",
                        gruppe = "Extension",
                        hint =
                            "A mathematical operator that is applied to the image data before an encoding scheme is applied."
                    }
                },
                {
                    318, new TifTag
                    {
                        name = "WhitePoint",
                        gruppe = "Extension",
                        hint = "The chromaticity of the white point of the image."
                    }
                },
                {
                    319, new TifTag
                    {
                        name = "PrimaryChromaticities",
                        gruppe = "Extension",
                        hint = "The chromaticities of the primaries of the image."
                    }
                },
                {
                    321, new TifTag
                    {
                        name = "HalftoneHints",
                        gruppe = "Extension",
                        hint =
                            "Conveys to the halftone function the range of gray levels within a colorimetrically-specified image that should retain tonal detail."
                    }
                },
                {
                    322, new TifTag
                    {
                        name = "TileWidth",
                        gruppe = "Extension",
                        hint =
                            "The tile width in pixels. This is the number of columns in each tile."
                    }
                },
                {
                    323, new TifTag
                    {
                        name = "TileLength",
                        gruppe = "Extension",
                        hint =
                            "The tile length (height) in pixels. This is the number of rows in each tile."
                    }
                },
                {
                    324, new TifTag
                    {
                        name = "TileOffsets",
                        gruppe = "Extension",
                        hint =
                            "For each tile, the byte offset of that tile, as compressed and stored on disk."
                    }
                },
                {
                    325, new TifTag
                    {
                        name = "TileByteCounts",
                        gruppe = "Extension",
                        hint =
                            "For each tile, the number of (compressed) bytes in that tile."
                    }
                },
                {
                    326, new TifTag
                    {
                        name = "BadFaxLines",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-F standard, denotes the number of 'bad' scan lines encountered by the facsimile device."
                    }
                },
                {
                    327, new TifTag
                    {
                        name = "CleanFaxData",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-F standard, indicates if 'bad' lines encountered during reception are stored in the data, or if 'bad' lines have been replaced by the receiver."
                    }
                },
                {
                    328, new TifTag
                    {
                        name = "ConsecutiveBadFaxLines",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-F standard, denotes the maximum number of consecutive 'bad' scanlines received."
                    }
                },
                {
                    330,
                    new TifTag
                    {
                        name = "SubIFDs",
                        gruppe = "Extension",
                        hint = "Offset to child IFDs."
                    }
                },
                {
                    332, new TifTag
                    {
                        name = "InkSet",
                        gruppe = "Extension",
                        hint =
                            "The set of inks used in a separated (PhotometricInterpretation=5) image."
                    }
                },
                {
                    333, new TifTag
                    {
                        name = "InkNames",
                        gruppe = "Extension",
                        hint = "The name of each ink used in a separated image."
                    }
                },
                {
                    334,
                    new TifTag
                    {
                        name = "NumberOfInks",
                        gruppe = "Extension",
                        hint = "The number of inks."
                    }
                },
                {
                    336, new TifTag
                    {
                        name = "DotRange",
                        gruppe = "Extension",
                        hint =
                            "The component values that correspond to a 0% dot and 100% dot."
                    }
                },
                {
                    337, new TifTag
                    {
                        name = "TargetPrinter",
                        gruppe = "Extension",
                        hint =
                            "A description of the printing environment for which this separation is intended."
                    }
                },
                {
                    339, new TifTag
                    {
                        name = "SampleFormat",
                        gruppe = "Extension",
                        hint =
                            "Specifies how to interpret each data sample in a pixel."
                    }
                },
                {
                    340, new TifTag
                    {
                        name = "SMinSampleValue",
                        gruppe = "Extension",
                        hint = "Specifies the minimum sample value."
                    }
                },
                {
                    341, new TifTag
                    {
                        name = "SMaxSampleValue",
                        gruppe = "Extension",
                        hint = "Specifies the maximum sample value."
                    }
                },
                {
                    342, new TifTag
                    {
                        name = "TransferRange",
                        gruppe = "Extension",
                        hint = "Expands the range of the TransferFunction."
                    }
                },
                {
                    343, new TifTag
                    {
                        name = "ClipPath",
                        gruppe = "Extension",
                        hint =
                            "Mirrors the essentials of PostScript's path creation functionality."
                    }
                },
                {
                    344, new TifTag
                    {
                        name = "XClipPathUnits",
                        gruppe = "Extension",
                        hint =
                            "The number of units that span the width of the image, in terms of integer ClipPath coordinates."
                    }
                },
                {
                    345, new TifTag
                    {
                        name = "YClipPathUnits",
                        gruppe = "Extension",
                        hint =
                            "The number of units that span the height of the image, in terms of integer ClipPath coordinates."
                    }
                },
                {
                    346, new TifTag
                    {
                        name = "Indexed",
                        gruppe = "Extension",
                        hint =
                            "Aims to broaden the support for indexed images to include support for any color space."
                    }
                },
                {
                    347, new TifTag
                    {
                        name = "JPEGTables",
                        gruppe = "Extension",
                        hint = "JPEG quantization and/or Huffman tables."
                    }
                },
                {
                    351,
                    new TifTag
                    {
                        name = "OPIProxy",
                        gruppe = "Extension",
                        hint = "OPI-related."
                    }
                },
                {
                    400, new TifTag
                    {
                        name = "GlobalParametersIFD",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-FX standard to point to an IFD containing tags that are globally applicable to the complete TIFF file."
                    }
                },
                {
                    401, new TifTag
                    {
                        name = "ProfileType",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-FX standard, denotes the type of data stored in this file or IFD."
                    }
                },
                {
                    402, new TifTag
                    {
                        name = "FaxProfile",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-FX standard, denotes the 'profile' that applies to this file."
                    }
                },
                {
                    403, new TifTag
                    {
                        name = "CodingMethods",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-FX standard, indicates which coding methods are used in the file."
                    }
                },
                {
                    404, new TifTag
                    {
                        name = "VersionYear",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-FX standard, denotes the year of the standard specified by the FaxProfile field."
                    }
                },
                {
                    405, new TifTag
                    {
                        name = "ModeNumber",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-FX standard, denotes the mode of the standard specified by the FaxProfile field."
                    }
                },
                {
                    433, new TifTag
                    {
                        name = "Decode",
                        gruppe = "Extension",
                        hint =
                            "Used in the TIFF-F and TIFF-FX standards, holds information about the ITULAB (PhotometricInterpretation = 10) encoding."
                    }
                },
                {
                    434, new TifTag
                    {
                        name = "DefaultImageColor",
                        gruppe = "Extension",
                        hint =
                            "Defined in the Mixed Raster Content part of RFC 2301, is the default color needed in areas where no image is available."
                    }
                },
                {
                    512, new TifTag
                    {
                        name = "JPEGProc",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    513, new TifTag
                    {
                        name = "JPEGInterchangeFormat",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    514, new TifTag
                    {
                        name = "JPEGInterchangeFormatLength",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    515, new TifTag
                    {
                        name = "JPEGRestartInterval",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    517, new TifTag
                    {
                        name = "JPEGLosslessPredictors",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    518, new TifTag
                    {
                        name = "JPEGPointTransforms",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    519, new TifTag
                    {
                        name = "JPEGQTables",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    520, new TifTag
                    {
                        name = "JPEGDCTables",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    521, new TifTag
                    {
                        name = "JPEGACTables",
                        gruppe = "Extension",
                        hint =
                            "Old-style JPEG compression field. TechNote2 invalidates this part of the specification."
                    }
                },
                {
                    529, new TifTag
                    {
                        name = "YCbCrCoefficients",
                        gruppe = "Extension",
                        hint = "The transformation from RGB to YCbCr image data."
                    }
                },
                {
                    530, new TifTag
                    {
                        name = "YCbCrSubSampling",
                        gruppe = "Extension",
                        hint =
                            "Specifies the subsampling factors used for the chrominance components of a YCbCr image."
                    }
                },
                {
                    531, new TifTag
                    {
                        name = "YCbCrPositioning",
                        gruppe = "Extension",
                        hint =
                            "Specifies the positioning of subsampled chrominance components relative to luminance samples."
                    }
                },
                {
                    532, new TifTag
                    {
                        name = "ReferenceBlackWhite",
                        gruppe = "Extension",
                        hint =
                            "Specifies a pair of headroom and footroom image data values (codes) for each pixel component."
                    }
                },
                {
                    559, new TifTag
                    {
                        name = "StripRowCounts",
                        gruppe = "Extension",
                        hint =
                            "Defined in the Mixed Raster Content part of RFC 2301, used to replace RowsPerStrip for IFDs with variable-sized strips."
                    }
                },
                {
                    700,
                    new TifTag
                    {
                        name = "XMP",
                        gruppe = "Extension",
                        hint = "XML packet containing XMP metadata"
                    }
                },
                {
                    32781,
                    new TifTag {name = "ImageID", gruppe = "Extension", hint = "OPI-related."}
                },
                {
                    34732, new TifTag
                    {
                        name = "ImageLayer",
                        gruppe = "Extension",
                        hint =
                            "Defined in the Mixed Raster Content part of RFC 2301, used to denote the particular function of this Image in the mixed raster scheme."
                    }
                },
                {
                    32932, new TifTag
                    {
                        name = "Wang Annotation",
                        gruppe = "Private",
                        hint = "Annotation data, as used in 'Imaging for Windows'."
                    }
                },
                {
                    33445, new TifTag
                    {
                        name = "MD FileTag",
                        gruppe = "Private",
                        hint =
                            "Specifies the pixel data format encoding in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33446, new TifTag
                    {
                        name = "MD ScalePixel",
                        gruppe = "Private",
                        hint =
                            "Specifies a scale factor in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33447, new TifTag
                    {
                        name = "MD ColorTable",
                        gruppe = "Private",
                        hint =
                            "Used to specify the conversion from 16bit to 8bit in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33448, new TifTag
                    {
                        name = "MD LabName",
                        gruppe = "Private",
                        hint =
                            "Name of the lab that scanned this file, as used in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33449, new TifTag
                    {
                        name = "MD SampleInfo",
                        gruppe = "Private",
                        hint =
                            "Information about the sample, as used in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33450, new TifTag
                    {
                        name = "MD PrepDate",
                        gruppe = "Private",
                        hint =
                            "Date the sample was prepared, as used in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33451, new TifTag
                    {
                        name = "MD PrepTime",
                        gruppe = "Private",
                        hint =
                            "Time the sample was prepared, as used in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33452, new TifTag
                    {
                        name = "MD FileUnits",
                        gruppe = "Private",
                        hint =
                            "Units for data in this file, as used in the Molecular Dynamics GEL file format."
                    }
                },
                {
                    33550, new TifTag
                    {
                        name = "ModelPixelScaleTag",
                        gruppe = "Private",
                        hint = "Used in interchangeable GeoTIFF files."
                    }
                },
                {
                    33723, new TifTag
                    {
                        name = "IPTC",
                        gruppe = "Private",
                        hint =
                            "IPTC (International Press Telecommunications Council) metadata."
                    }
                },
                {
                    33918, new TifTag
                    {
                        name = "INGR Packet Data Tag",
                        gruppe = "Private",
                        hint = "Intergraph Application specific storage."
                    }
                },
                {
                    33919, new TifTag
                    {
                        name = "INGR Flag Registers",
                        gruppe = "Private",
                        hint = "Intergraph Application specific flags."
                    }
                },
                {
                    33920, new TifTag
                    {
                        name = "IrasB Transformation Matrix",
                        gruppe = "Private",
                        hint =
                            "Originally part of Intergraph's GeoTIFF tags, but likely understood by IrasB only."
                    }
                },
                {
                    33922, new TifTag
                    {
                        name = "ModelTiepointTag",
                        gruppe = "Private",
                        hint =
                            "Originally part of Intergraph's GeoTIFF tags, but now used in interchangeable GeoTIFF files."
                    }
                },
                {
                    34264, new TifTag
                    {
                        name = "ModelTransformationTag",
                        gruppe = "Private",
                        hint = "Used in interchangeable GeoTIFF files."
                    }
                },
                {
                    34377, new TifTag
                    {
                        name = "Photoshop",
                        gruppe = "Private",
                        hint = "Collection of Photoshop 'Image Resource Blocks'."
                    }
                },
                {
                    34665,
                    new TifTag
                    {
                        name = "Exif IFD",
                        gruppe = "Private",
                        hint = "A pointer to the Exif IFD."
                    }
                },
                {
                    34675,
                    new TifTag
                    {
                        name = "ICC Profile",
                        gruppe = "Private",
                        hint = "ICC profile data."
                    }
                },
                {
                    34735, new TifTag
                    {
                        name = "GeoKeyDirectoryTag",
                        gruppe = "Private",
                        hint = "Used in interchangeable GeoTIFF files."
                    }
                },
                {
                    34736, new TifTag
                    {
                        name = "GeoDoubleParamsTag",
                        gruppe = "Private",
                        hint = "Used in interchangeable GeoTIFF files."
                    }
                },
                {
                    34737, new TifTag
                    {
                        name = "GeoAsciiParamsTag",
                        gruppe = "Private",
                        hint = "Used in interchangeable GeoTIFF files."
                    }
                },
                {
                    34853, new TifTag
                    {
                        name = "GPS IFD",
                        gruppe = "Private",
                        hint = "A pointer to the Exif-related GPS Info IFD."
                    }
                },
                {
                    34908,
                    new TifTag
                    {
                        name = "HylaFAX FaxRecvParams",
                        gruppe = "Private",
                        hint = "Used by HylaFAX."
                    }
                },
                {
                    34909,
                    new TifTag
                    {
                        name = "HylaFAX FaxSubAddress",
                        gruppe = "Private",
                        hint = "Used by HylaFAX."
                    }
                },
                {
                    34910,
                    new TifTag
                    {
                        name = "HylaFAX FaxRecvTime",
                        gruppe = "Private",
                        hint = "Used by HylaFAX."
                    }
                },
                {
                    37724,
                    new TifTag
                    {
                        name = "ImageSourceData",
                        gruppe = "Private",
                        hint = "Used by Adobe Photoshop."
                    }
                },
                {
                    40965, new TifTag
                    {
                        name = "Interoperability IFD",
                        gruppe = "Private",
                        hint = "A pointer to the Exif-related Interoperability IFD."
                    }
                },
                {
                    42112, new TifTag
                    {
                        name = "GDAL_METADATA",
                        gruppe = "Private",
                        hint =
                            "Used by the GDAL library, holds an XML list of name=value 'metadata' values about the image as a whole, and about specific samples."
                    }
                },
                {
                    42113, new TifTag
                    {
                        name = "GDAL_NODATA",
                        gruppe = "Private",
                        hint =
                            "Used by the GDAL library, contains an ASCII encoded nodata or background pixel value."
                    }
                },
                {
                    50215, new TifTag
                    {
                        name = "Oce Scanjob Description",
                        gruppe = "Private",
                        hint = "Used in the Oce scanning process."
                    }
                },
                {
                    50216, new TifTag
                    {
                        name = "Oce Application Selector",
                        gruppe = "Private",
                        hint = "Used in the Oce scanning process."
                    }
                },
                {
                    50217, new TifTag
                    {
                        name = "Oce Identification Number",
                        gruppe = "Private",
                        hint = "Used in the Oce scanning process."
                    }
                },
                {
                    50218, new TifTag
                    {
                        name = "Oce ImageLogic Characteristics",
                        gruppe = "Private",
                        hint = "Used in the Oce scanning process."
                    }
                },
                {
                    50706,
                    new TifTag
                    {
                        name = "DNGVersion",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50707,
                    new TifTag
                    {
                        name = "DNGBackwardVersion",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50708,
                    new TifTag
                    {
                        name = "UniqueCameraModel",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50709,
                    new TifTag
                    {
                        name = "LocalizedCameraModel",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50710,
                    new TifTag
                    {
                        name = "CFAPlaneColor",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50711,
                    new TifTag
                    {
                        name = "CFALayout",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50712,
                    new TifTag
                    {
                        name = "LinearizationTable",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50713,
                    new TifTag
                    {
                        name = "BlackLevelRepeatDim",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50714,
                    new TifTag
                    {
                        name = "BlackLevel",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50715,
                    new TifTag
                    {
                        name = "BlackLevelDeltaH",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50716,
                    new TifTag
                    {
                        name = "BlackLevelDeltaV",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50717,
                    new TifTag
                    {
                        name = "WhiteLevel",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50718,
                    new TifTag
                    {
                        name = "DefaultScale",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50719,
                    new TifTag
                    {
                        name = "DefaultCropOrigin",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50720,
                    new TifTag
                    {
                        name = "DefaultCropSize",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50721,
                    new TifTag
                    {
                        name = "ColorMatrix1",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50722,
                    new TifTag
                    {
                        name = "ColorMatrix2",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50723,
                    new TifTag
                    {
                        name = "CameraCalibration1",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50724,
                    new TifTag
                    {
                        name = "CameraCalibration2",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50725,
                    new TifTag
                    {
                        name = "ReductionMatrix1",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50726,
                    new TifTag
                    {
                        name = "ReductionMatrix2",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50727,
                    new TifTag
                    {
                        name = "AnalogBalance",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50728,
                    new TifTag
                    {
                        name = "AsShotNeutral",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50729,
                    new TifTag
                    {
                        name = "AsShotWhiteXY",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50730,
                    new TifTag
                    {
                        name = "BaselineExposure",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50731,
                    new TifTag
                    {
                        name = "BaselineNoise",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50732,
                    new TifTag
                    {
                        name = "BaselineSharpness",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50733,
                    new TifTag
                    {
                        name = "BayerGreenSplit",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50734,
                    new TifTag
                    {
                        name = "LinearResponseLimit",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50735,
                    new TifTag
                    {
                        name = "CameraSerialNumber",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50736,
                    new TifTag
                    {
                        name = "LensInfo",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50737,
                    new TifTag
                    {
                        name = "ChromaBlurRadius",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50738,
                    new TifTag
                    {
                        name = "AntiAliasStrength",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50740,
                    new TifTag
                    {
                        name = "DNGPrivateData",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50741,
                    new TifTag
                    {
                        name = "MakerNoteSafety",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50778,
                    new TifTag
                    {
                        name = "CalibrationIlluminant1",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50779,
                    new TifTag
                    {
                        name = "CalibrationIlluminant2",
                        gruppe = "Private",
                        hint = "Used in IFD 0 of DNG files."
                    }
                },
                {
                    50780,
                    new TifTag
                    {
                        name = "BestQualityScale",
                        gruppe = "Private",
                        hint = "Used in Raw IFD of DNG files."
                    }
                },
                {
                    50784, new TifTag
                    {
                        name = "Alias Layer Metadata",
                        gruppe = "Private",
                        hint = "Alias Sketchbook Pro layer usage description."
                    }
                },
                {
                    37679, new TifTag
                    {
                        name = "Microsoft ukendt navn1",
                        gruppe = "Private",
                        hint =
                            "Contains plain text reflecting the contents of the page image in the TIFF file"
                    }
                },
                {
                    37681, new TifTag
                    {
                        name = "Microsoft ukendt navn2",
                        gruppe = "Private",
                        hint =
                            "Contains positioning information which describes where the text from Tag 37679 appears on the page, as well as information about the position of other objects such as images, tables, and hyphens. The information in this tag is used by the MODI application to enable its text selection feature"
                    }
                },
                {
                    37680, new TifTag
                    {
                        name = "Microsoft ukendt navn3",
                        gruppe = "Private",
                        hint =
                            "Contains a binary dump of an OLE Property Set Storage"
                    }
                },
                {
                    33434,
                    new TifTag
                    {
                        name = "ExposureTime",
                        gruppe = "Exif",
                        hint = "Exposure time, given in seconds."
                    }
                },
                {
                    33437,
                    new TifTag {name = "FNumber", gruppe = "Exif", hint = "The F number."}
                },
                {
                    34850, new TifTag
                    {
                        name = "ExposureProgram",
                        gruppe = "Exif",
                        hint =
                            "The class of the program used by the camera to set exposure when the picture is taken."
                    }
                },
                {
                    34852, new TifTag
                    {
                        name = "SpectralSensitivity",
                        gruppe = "Exif",
                        hint =
                            "Indicates the spectral sensitivity of each channel of the camera used."
                    }
                },
                {
                    34855, new TifTag
                    {
                        name = "ISOSpeedRatings",
                        gruppe = "Exif",
                        hint =
                            "Indicates the ISO Speed and ISO Latitude of the camera or input device as specified in ISO 12232."
                    }
                },
                {
                    34856, new TifTag
                    {
                        name = "OECF",
                        gruppe = "Exif",
                        hint =
                            "Indicates the Opto-Electric Conversion Function (OECF) specified in ISO 14524."
                    }
                },
                {
                    36864, new TifTag
                    {
                        name = "ExifVersion",
                        gruppe = "Exif",
                        hint = "The version of the supported Exif standard."
                    }
                },
                {
                    36867, new TifTag
                    {
                        name = "DateTimeOriginal",
                        gruppe = "Exif",
                        hint =
                            "The date and time when the original image data was generated."
                    }
                },
                {
                    36868, new TifTag
                    {
                        name = "DateTimeDigitized",
                        gruppe = "Exif",
                        hint =
                            "The date and time when the image was stored as digital data."
                    }
                },
                {
                    37121, new TifTag
                    {
                        name = "ComponentsConfiguration",
                        gruppe = "Exif",
                        hint =
                            "Specific to compressed data; specifies the channels and complements PhotometricInterpretation"
                    }
                },
                {
                    37122, new TifTag
                    {
                        name = "CompressedBitsPerPixel",
                        gruppe = "Exif",
                        hint =
                            "Specific to compressed data; states the compressed bits per pixel."
                    }
                },
                {
                    37377,
                    new TifTag
                    {
                        name = "ShutterSpeedValue",
                        gruppe = "Exif",
                        hint = "Shutter speed."
                    }
                },
                {
                    37378,
                    new TifTag
                    {
                        name = "ApertureValue",
                        gruppe = "Exif",
                        hint = "The lens aperture."
                    }
                },
                {
                    37379,
                    new TifTag
                    {
                        name = "BrightnessValue",
                        gruppe = "Exif",
                        hint = "The value of brightness."
                    }
                },
                {
                    37380,
                    new TifTag
                    {
                        name = "ExposureBiasValue",
                        gruppe = "Exif",
                        hint = "The exposure bias."
                    }
                },
                {
                    37381,
                    new TifTag
                    {
                        name = "MaxApertureValue",
                        gruppe = "Exif",
                        hint = "The smallest F number of the lens."
                    }
                },
                {
                    37382, new TifTag
                    {
                        name = "SubjectDistance",
                        gruppe = "Exif",
                        hint = "The distance to the subject, given in meters."
                    }
                },
                {
                    37383,
                    new TifTag
                    {
                        name = "MeteringMode",
                        gruppe = "Exif",
                        hint = "The metering mode."
                    }
                },
                {
                    37384,
                    new TifTag
                    {
                        name = "LightSource",
                        gruppe = "Exif",
                        hint = "The kind of light source."
                    }
                },
                {
                    37385, new TifTag
                    {
                        name = "Flash",
                        gruppe = "Exif",
                        hint =
                            "Indicates the status of flash when the image was shot."
                    }
                },
                {
                    37386, new TifTag
                    {
                        name = "FocalLength",
                        gruppe = "Exif",
                        hint = "The actual focal length of the lens, in mm."
                    }
                },
                {
                    37396, new TifTag
                    {
                        name = "SubjectArea",
                        gruppe = "Exif",
                        hint =
                            "Indicates the location and area of the main subject in the overall scene."
                    }
                },
                {
                    37500,
                    new TifTag
                    {
                        name = "MakerNote",
                        gruppe = "Exif",
                        hint = "Manufacturer specific information."
                    }
                },
                {
                    37510, new TifTag
                    {
                        name = "UserComment",
                        gruppe = "Exif",
                        hint =
                            "Keywords or comments on the image; complements ImageDescription."
                    }
                },
                {
                    37520, new TifTag
                    {
                        name = "SubsecTime",
                        gruppe = "Exif",
                        hint =
                            "A tag used to record fractions of seconds for the DateTime tag."
                    }
                },
                {
                    37521, new TifTag
                    {
                        name = "SubsecTimeOriginal",
                        gruppe = "Exif",
                        hint =
                            "A tag used to record fractions of seconds for the DateTimeOriginal tag."
                    }
                },
                {
                    37522, new TifTag
                    {
                        name = "SubsecTimeDigitized",
                        gruppe = "Exif",
                        hint =
                            "A tag used to record fractions of seconds for the DateTimeDigitized tag."
                    }
                },
                {
                    40960, new TifTag
                    {
                        name = "FlashpixVersion",
                        gruppe = "Exif",
                        hint =
                            "The Flashpix format version supported by a FPXR file."
                    }
                },
                {
                    40961, new TifTag
                    {
                        name = "ColorSpace",
                        gruppe = "Exif",
                        hint =
                            "The color space information tag is always recorded as the color space specifier."
                    }
                },
                {
                    40962, new TifTag
                    {
                        name = "PixelXDimension",
                        gruppe = "Exif",
                        hint =
                            "Specific to compressed data; the valid width of the meaningful image."
                    }
                },
                {
                    40963, new TifTag
                    {
                        name = "PixelYDimension",
                        gruppe = "Exif",
                        hint =
                            "Specific to compressed data; the valid height of the meaningful image."
                    }
                },
                {
                    40964, new TifTag
                    {
                        name = "RelatedSoundFile",
                        gruppe = "Exif",
                        hint =
                            "Used to record the name of an audio file related to the image data."
                    }
                },
                {
                    41483, new TifTag
                    {
                        name = "FlashEnergy",
                        gruppe = "Exif",
                        hint =
                            "Indicates the strobe energy at the time the image is captured, as measured in Beam Candle Power Seconds"
                    }
                },
                {
                    41484, new TifTag
                    {
                        name = "SpatialFrequencyResponse",
                        gruppe = "Exif",
                        hint =
                            "Records the camera or input device spatial frequency table and SFR values in the direction of image width, image height, and diagonal direction, as specified in ISO 12233."
                    }
                },
                {
                    41486, new TifTag
                    {
                        name = "FocalPlaneXResolution",
                        gruppe = "Exif",
                        hint =
                            "Indicates the number of pixels in the image width (X) direction per FocalPlaneResolutionUnit on the camera focal plane."
                    }
                },
                {
                    41487, new TifTag
                    {
                        name = "FocalPlaneYResolution",
                        gruppe = "Exif",
                        hint =
                            "Indicates the number of pixels in the image height (Y) direction per FocalPlaneResolutionUnit on the camera focal plane."
                    }
                },
                {
                    41488, new TifTag
                    {
                        name = "FocalPlaneResolutionUnit",
                        gruppe = "Exif",
                        hint =
                            "Indicates the unit for measuring FocalPlaneXResolution and FocalPlaneYResolution."
                    }
                },
                {
                    41492, new TifTag
                    {
                        name = "SubjectLocation",
                        gruppe = "Exif",
                        hint =
                            "Indicates the location of the main subject in the scene."
                    }
                },
                {
                    41493, new TifTag
                    {
                        name = "ExposureIndex",
                        gruppe = "Exif",
                        hint =
                            "Indicates the exposure index selected on the camera or input device at the time the image is captured."
                    }
                },
                {
                    41495, new TifTag
                    {
                        name = "SensingMethod",
                        gruppe = "Exif",
                        hint =
                            "Indicates the image sensor type on the camera or input device."
                    }
                },
                {
                    41728,
                    new TifTag
                    {
                        name = "FileSource",
                        gruppe = "Exif",
                        hint = "Indicates the image source."
                    }
                },
                {
                    41729,
                    new TifTag
                    {
                        name = "SceneType",
                        gruppe = "Exif",
                        hint = "Indicates the type of scene."
                    }
                },
                {
                    41730, new TifTag
                    {
                        name = "CFAPattern",
                        gruppe = "Exif",
                        hint =
                            "Indicates the color filter array (CFA) geometric pattern of the image sensor when a one-chip color area sensor is used."
                    }
                },
                {
                    41985, new TifTag
                    {
                        name = "CustomRendered",
                        gruppe = "Exif",
                        hint =
                            "Indicates the use of special processing on image data, such as rendering geared to output."
                    }
                },
                {
                    41986, new TifTag
                    {
                        name = "ExposureMode",
                        gruppe = "Exif",
                        hint =
                            "Indicates the exposure mode set when the image was shot."
                    }
                },
                {
                    41987, new TifTag
                    {
                        name = "WhiteBalance",
                        gruppe = "Exif",
                        hint =
                            "Indicates the white balance mode set when the image was shot."
                    }
                },
                {
                    41988, new TifTag
                    {
                        name = "DigitalZoomRatio",
                        gruppe = "Exif",
                        hint =
                            "Indicates the digital zoom ratio when the image was shot."
                    }
                },
                {
                    41989, new TifTag
                    {
                        name = "FocalLengthIn35mmFilm",
                        gruppe = "Exif",
                        hint =
                            "Indicates the equivalent focal length assuming a 35mm film camera, in mm."
                    }
                },
                {
                    41990, new TifTag
                    {
                        name = "SceneCaptureType",
                        gruppe = "Exif",
                        hint = "Indicates the type of scene that was shot."
                    }
                },
                {
                    41991, new TifTag
                    {
                        name = "GainControl",
                        gruppe = "Exif",
                        hint =
                            "Indicates the degree of overall image gain adjustment."
                    }
                },
                {
                    41992, new TifTag
                    {
                        name = "Contrast",
                        gruppe = "Exif",
                        hint =
                            "Indicates the direction of contrast processing applied by the camera when the image was shot."
                    }
                },
                {
                    41993, new TifTag
                    {
                        name = "Saturation",
                        gruppe = "Exif",
                        hint =
                            "Indicates the direction of saturation processing applied by the camera when the image was shot."
                    }
                },
                {
                    41994, new TifTag
                    {
                        name = "Sharpness",
                        gruppe = "Exif",
                        hint =
                            "Indicates the direction of sharpness processing applied by the camera when the image was shot."
                    }
                },
                {
                    41995, new TifTag
                    {
                        name = "DeviceSettingDescription",
                        gruppe = "Exif",
                        hint =
                            "This tag indicates information on the picture-taking conditions of a particular camera model."
                    }
                },
                {
                    41996, new TifTag
                    {
                        name = "SubjectDistanceRange",
                        gruppe = "Exif",
                        hint = "Indicates the distance to the subject."
                    }
                },
                {
                    42016, new TifTag
                    {
                        name = "ImageUniqueID",
                        gruppe = "Exif",
                        hint =
                            "Indicates an identifier assigned uniquely to each image."
                    }
                }
            };
        }

        #endregion
    }
}