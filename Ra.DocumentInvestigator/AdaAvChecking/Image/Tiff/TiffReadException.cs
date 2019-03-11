namespace Ra.DocumentInvestigator.AdaAvChecking.Image.Tiff
{
    #region Namespace Using

    using System;
    using System.Reflection;
    using log4net;

    #endregion

    internal class TiffReadException : Exception
    {
        #region  Fields

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region  Constructors

        public TiffReadException(string filename) : this(filename, null)
        {
        }

        public TiffReadException(string filename, Exception innerException) : base("", innerException)
        {
            Filename = filename;

            _log.ErrorFormat($"'{filename}' could not be read");
        }

        #endregion

        #region Properties

        public string Filename { get; }

        #endregion
    }
}