namespace Ra.DocumentInvestigator
{
    #region Namespace Using

    using BitMiracle.LibTiff.Classic;

    #endregion

    public class KeepQuietWarning : TiffErrorHandler
    {
        #region

        public override void WarningHandler(Tiff tif, string method, string format, params object[] args)
        {
//            base.WarningHandler(tif, method, format, args);
        }


        public override void WarningHandlerExt(Tiff tif, object clientData, string method, string format, params object[] args)
        {
//            base.WarningHandlerExt(tif, clientData, method, format, args);
        }

        #endregion
    }
}