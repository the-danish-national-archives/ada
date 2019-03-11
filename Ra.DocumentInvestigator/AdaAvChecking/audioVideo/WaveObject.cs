namespace Ra.DocumentInvestigator.AdaAvChecking.Image.Jp2
{
    #region Namespace Using

    using System;
    using audioVideo;
    using AdaReporting;
    using Common;
    using filetypes;
    using Tiff;

    #endregion

    public class WaveObject : BaseDocObject
    {
        #region  Fields

        private readonly TiffChecks _settings;

        #endregion

        #region  Constructors

        public WaveObject
        (
            BufferedProgressStream currentStream,
            DocInfo docInfo,
            Action<DocLogEntry> callback = null,
            int settings = (int) TiffChecks.ALL)
            : base(currentStream, docInfo, callback)
        {
            _settings = (TiffChecks) settings;
            if (typeDetector.VerifyFiletypeByMagicNumberBasedOnExtension(Name) != LegalFileType.Wav)
            {
                var logEvent = new DocLogEntryF2(DocInfo);
                Callback(logEvent);
                return;
            }


            if ((_settings & TiffChecks.COMPRESSION) != 0)
                switch (MediaProcessor.TestAudioKompressionByImageInfo(currentStream.file.FullName))
                {
                    case AudioCompressionTestvalue.LydStreamsErOK:
                        break;
                    case AudioCompressionTestvalue.LydUlovlig:
                    case AudioCompressionTestvalue.UgyldigDLLVersion:
                    case AudioCompressionTestvalue.UkendtFejl:
                    case AudioCompressionTestvalue.IngenStreams:
                    default:
                        Callback(new DocLogEntryF2(docInfo));
                        break;
                }
        }

        #endregion

        #region

//        public override uint PageCount { get; }

        public override void ParseInformation()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}