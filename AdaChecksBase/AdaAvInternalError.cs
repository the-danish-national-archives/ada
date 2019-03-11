namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;

    #endregion

    public class AdaAvInternalError : AdaAvCheckNotification
    {
        #region  Constructors

        public AdaAvInternalError(Exception e, Type module)
            : base("0.1")
        {
            var message = e.Message;
            if (e.InnerException != null) message += ": " + e.InnerException.Message;
            Exception = message;
            Module = module.ToString();
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Exception { get; set; }

        [AdaAvCheckNotificationTag]
        public string Module { get; set; }

        #endregion
    }
}