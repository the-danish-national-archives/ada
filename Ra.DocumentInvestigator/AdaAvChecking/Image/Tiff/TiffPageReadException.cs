namespace Ra.DocumentInvestigator.AdaAvChecking.Image.Tiff
{
    #region Namespace Using

    using System;
    using System.Reflection;
    using log4net;

    #endregion

    internal class TiffPageReadException : TiffReadException
    {
        #region  Fields

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region  Constructors

        public TiffPageReadException(int pageNo, string filename, Exception exception)
            : base(filename)
        {
            PageNo = pageNo;

            _log.ErrorFormat($"Tiffpage {pageNo} in '{filename}' could not be read");
        }

        #endregion

        #region Properties

        public int PageNo { get; }

        #endregion
    }
}