namespace Ra.DocumentInvestigator.AdaAvChecking.Image.Jp2
{
    #region Namespace Using

    using System;
    using AdaReporting;
    using Common;
    using filetypes;

    #endregion

    public class Jp2Object : BaseDocObject
    {
        #region  Constructors

        public Jp2Object(BufferedProgressStream currentStream, DocInfo docInfo, Action<DocLogEntry> callback = null)
            : base(currentStream, docInfo, callback)
        {
            IsJp2 = FileType == LegalFileType.Jp2;
            if (IsJp2)
            {
//                this.PageCount = ReadPageCount();
//
//                //this.Name = this._currentStream.file.Name;
//                LoadInformation();
            }
            else
            {
                var logEvent = new DocLogEntryE12(DocInfo);
                Callback(logEvent);
            }
        }

        #endregion

        #region Properties

        public bool IsJp2 { get; }

        #endregion

        #region

        public override void ParseInformation()
        {
            throw new NotImplementedException();
        }

        #endregion

//        private void LoadInformation()
//        {
//            throw new NotImplementedException();
//        }
//
//        protected void TestsWithImageInfo(ref PageObject docPage, int i, CodecsImageInfo info)
//        {
//            docPage.OddDBI = this.TestIfDPIIsOdd(info);
//            docPage.Compression = this.GetPageCompression(info);
//        }
    }
}