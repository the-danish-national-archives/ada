namespace Ra.DocumentInvestigator.OldForWinforms
{
    #region Namespace Using

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class NoPrintersFoundException : Exception
    {
        #region  Constructors

        public NoPrintersFoundException()
        {
        }

        public NoPrintersFoundException(string message)
            : base(message)
        {
        }

        public NoPrintersFoundException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public NoPrintersFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NoPrintersFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected NoPrintersFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}