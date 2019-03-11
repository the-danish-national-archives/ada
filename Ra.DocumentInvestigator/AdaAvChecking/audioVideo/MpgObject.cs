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

    public class MpgObject : BaseDocObject
    {
        #region  Fields

        private readonly TiffChecks _settings;

        #endregion

        #region  Constructors

        public MpgObject
        (
            BufferedProgressStream currentStream,
            DocInfo docInfo,
            Action<DocLogEntry> callback = null,
            int settings = (int) TiffChecks.ALL)
            : base(currentStream, docInfo, callback)
        {
            _settings = (TiffChecks) settings;
            if (typeDetector.VerifyFiletypeByMagicNumberBasedOnExtension(Name) == LegalFileType.Mpg)
            {
                var logEvent = new DocLogEntryF4(DocInfo);
                Callback(logEvent);
                return;
            }

            if ((_settings & TiffChecks.COMPRESSION) != 0)
                switch (MediaProcessor.TestVideoKompressionByImageInfo(FileFullName))
                {
                    case VideoCompressionTestvalue.AudioAndVideoOK:
                        break;
                    case VideoCompressionTestvalue.AudioAndVideoIllegal:
                    case VideoCompressionTestvalue.VideoIllegal:
                    case VideoCompressionTestvalue.AudioIllegal:
                    case VideoCompressionTestvalue.InvalidDLLVersion:
                    case VideoCompressionTestvalue.UnknownError:
                    case VideoCompressionTestvalue.NoStreams:
                    default:
                        Callback(new DocLogEntryF3(DocInfo));
                        return;
                }

//            this.IsJp2 = this.FileType == LegalFileType.Jp2;
//            if (this.IsJp2)
//            {
////                this.PageCount = ReadPageCount();
////
////                //this.Name = this._currentStream.file.Name;
////                LoadInformation();
//            }
//            else
//            {
//                var logEvent = new DocLogEntryE12(DocInfo);
//                this.Callback(logEvent);
//            }
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