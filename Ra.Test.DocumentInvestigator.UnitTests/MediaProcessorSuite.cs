namespace Ra.Test.DocumentInvestigator.UnitTests
{
    #region Namespace Using

    using System.IO;
    using System.Reflection;
    using Common;
    using log4net;
    using NUnit.Framework;
    using Ra.DocumentInvestigator.AdaAvChecking.audioVideo;
    using Ra.DocumentInvestigator.AdaAvChecking.filetypes;
    using Utilities;

    #endregion

    [TestFixture]
    public class MediaProcessorSuite
    {
        private string base_path;

        private FileInfo mp4_file_info;

        private FileInfo mkv_file_info;

        private FileInfo mp3_classic_file_info;

        private FileInfo aac_rock_file_info;

        private FileInfo wav_piano_file_info;

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string FilesDir = "MediaFiles";

        public MediaProcessorSuite()
        {
            log.Debug("ctor");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            log.Debug("OneTimeSetUp");

            base_path = TestHelper.SetBasePath(FilesDir);
            mp4_file_info = new FileInfo(Path.Combine(base_path, "SampleVideo_1280x720_1mb.mp4"));
            mkv_file_info = new FileInfo(Path.Combine(base_path, "SampleVideo_720x480_1mb.mkv"));
            mp3_classic_file_info = new FileInfo(Path.Combine(base_path, "Classic.mp3"));
            wav_piano_file_info = new FileInfo(Path.Combine(base_path, "piano.wav"));
            aac_rock_file_info = new FileInfo(Path.Combine(base_path, "RockSample.aac"));
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            log.Debug("OneTimeTearDown");
        }

        [TestCase]
        public void TestTypenMP3()
        {
            var testObject = new TypeDetector();
            log.Debug($"Testing with file '{mp3_classic_file_info.FullName}'");
            if (!mp3_classic_file_info.Exists) Assert.Inconclusive($"File '{mp3_classic_file_info.FullName}' not found");
            using (var stream = new BufferedProgressStream(mp3_classic_file_info))
            {
                var result = testObject.VerifyMP3ByMagicNumber(stream);
                Assert.IsTrue(result);
            }
        }

        [TestCase]
        public void TestAudioKompressionMP3()
        {
            var result = MediaProcessor.TestAudioKompressionByImageInfo(mp3_classic_file_info.FullName);
            Assert.AreEqual(AudioCompressionTestvalue.LydStreamsErOK, result);
        }

        [TestCase]
        public void TestAudioKompressionWAV()
        {
            log.Debug($"Testing with file '{wav_piano_file_info.FullName}'");
            if (!wav_piano_file_info.Exists) Assert.Inconclusive($"File '{wav_piano_file_info.FullName}' not found");
            var result = MediaProcessor.TestAudioKompressionByImageInfo(wav_piano_file_info.FullName);
            Assert.AreEqual(AudioCompressionTestvalue.LydStreamsErOK, result);
        }

        [TestCase]
        public void TestAudioKompressionAAC()
        {
            //            var testObject = new MediaProcessor();
            var result = MediaProcessor.TestAudioKompressionByImageInfo(aac_rock_file_info.FullName);
            Assert.AreEqual(AudioCompressionTestvalue.LydStreamsErOK, result);
        }

        [TestCase]
        public void TestAudioKompressionMP4()
        {
            //            var testObject = new MediaProcessor();
            var result = MediaProcessor.TestAudioKompressionByImageInfo(mp4_file_info.FullName);
            Assert.AreEqual(AudioCompressionTestvalue.LydUlovlig, result);
        }

        [TestCase]
        public void TestVideoKompressionMKV()
        {
            //            var testObject = new MediaProcessor();
            var result = MediaProcessor.TestVideoKompressionByImageInfo(mp4_file_info.FullName);
            Assert.AreEqual(VideoCompressionTestvalue.AudioAndVideoOK, result);
        }

        [TestCase]
        public void TestVideoKompressionMP4()
        {
            //            var testObject = new MediaProcessor();
            var result = MediaProcessor.TestVideoKompressionByImageInfo(mkv_file_info.FullName);
            Assert.AreEqual(VideoCompressionTestvalue.AudioAndVideoOK, result);
        }
    }
}