namespace Ra.DocumentInvestigator.AdaAvChecking.AudioVideo
{
    #region Namespace Using

    using System;
    using audioVideo;
    using AdaReporting;
    using Common;
    using filetypes;
    using Image;
    using Image.Tiff;

    #endregion

    public class Mp3Object : BaseDocObject
    {
        #region  Fields

        private readonly TiffChecks _settings;

        #endregion

        #region  Constructors

        public Mp3Object
        (
            BufferedProgressStream currentStream,
            DocInfo docInfo,
            Action<DocLogEntry> callback = null,
            int settings = (int) TiffChecks.ALL)
            : base(currentStream, docInfo, callback)
        {
            _settings = (TiffChecks) settings;
            if (typeDetector.VerifyFiletypeByMagicNumberBasedOnExtension(Name) == LegalFileType.MP3)
            {
                var logEvent = new DocLogEntryF1(DocInfo);
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
                        Callback(new DocLogEntryF1(docInfo));
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