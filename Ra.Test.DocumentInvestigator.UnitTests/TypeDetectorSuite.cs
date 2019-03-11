namespace Ra.Test.DocumentInvestigator.UnitTests
{
    #region Namespace Using

    using System.IO;
    using System.Reflection;
    using log4net;
    using NUnit.Framework;
    using Ra.DocumentInvestigator.AdaAvChecking.filetypes;
    using Utilities;

    #endregion

    [TestFixture]
    public class TypeDetectorSuite
    {
        private readonly ILog log;

        private FileInfo mp3_file_info;

        private FileInfo tif_file_info;

        private FileInfo wav_file_info;

        private FileInfo xsd_file_info;

        private FileInfo jp2_file_info;

        private TypeDetector test_object;

        private const string FilesDir = "TestFiletypes";

        private string base_path;

        public TypeDetectorSuite()
        {
            LogTools.ConfigureWithConsole();

            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            log.Debug("OneTimeSetUp");
            base_path = TestHelper.SetBasePath(FilesDir);
            Directory.CreateDirectory(base_path);
            mp3_file_info = new FileInfo(Path.Combine(base_path, "mp3File.mp3"));
            tif_file_info = new FileInfo(Path.Combine(base_path, "tifFile.tif"));
            wav_file_info = new FileInfo(Path.Combine(base_path, "wavFile.wav"));
            xsd_file_info = new FileInfo(Path.Combine(base_path, "xsdFile.xsd"));
            jp2_file_info = new FileInfo(Path.Combine(base_path, "jp2File.jp2"));
            test_object = new TypeDetector();
        }

        [TestCase]
        public void TestMP3()
        {
            log.Debug($"Testing with file '{mp3_file_info.FullName}'");
            if (!mp3_file_info.Exists) Assert.Inconclusive($"File '{mp3_file_info.FullName}' not found");
            Assert.AreEqual(
                LegalFileType.MP3,
                test_object.VerifyFiletypeByMagicNumberBasedOnExtension(mp3_file_info.FullName));
        }

        [TestCase]
        public void Testtif()
        {
            Assert.AreEqual(
                LegalFileType.Tif,
                test_object.VerifyFiletypeByMagicNumberBasedOnExtension(tif_file_info.FullName));
        }


        [TestCase]
        public void Testwav()
        {
            Assert.AreEqual(
                LegalFileType.Wav,
                test_object.VerifyFiletypeByMagicNumberBasedOnExtension(wav_file_info.FullName));
        }

        [TestCase]
        public void Testxsd()
        {
            Assert.AreEqual(
                LegalFileType.XSD,
                test_object.VerifyFiletypeByMagicNumberBasedOnExtension(xsd_file_info.FullName));
        }

        [TestCase]
        public void Testjp2()
        {
            Assert.AreEqual(
                LegalFileType.Jp2,
                test_object.VerifyFiletypeByMagicNumberBasedOnExtension(jp2_file_info.FullName));
        }
    }
}