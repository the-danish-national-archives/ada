namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Runtime.Serialization;

    #endregion

    public class AdaSkipAllActionException : AdaException, ISerializable
    {
        #region  Constructors

        public AdaSkipAllActionException()
        {
        }

        public AdaSkipAllActionException(string message) : base(message)
        {
        }

        public AdaSkipAllActionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AdaSkipAllActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion
    }
}