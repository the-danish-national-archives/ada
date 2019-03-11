namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Runtime.Serialization;

    #endregion

    public class AdaSkipActionException : AdaException, ISerializable
    {
        #region  Constructors

        public AdaSkipActionException()
        {
        }

        public AdaSkipActionException(string message) : base(message)
        {
        }

        public AdaSkipActionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AdaSkipActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion
    }
}