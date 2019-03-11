namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Runtime.Serialization;

    #endregion

    public class AdaException : Exception, ISerializable
    {
        #region  Constructors

        public AdaException()
        {
        }

        public AdaException(string message) : base(message)
        {
        }

        public AdaException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AdaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion
    }
}