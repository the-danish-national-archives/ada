namespace Ada.ChecksBase
{
    #region Namespace Using

    using System.Collections.Generic;
    using Ra.Common.ExtensionMethods;

    #endregion

    public class AdaAvCheckNotificationTagSmartToStringAttribute : AdaAvCheckNotificationTagAttribute
    {
        #region Properties

        public string Seperator { get; set; } = ",";

        #endregion

        #region

        public override string ValueToString(object o)
        {
            return (o as IEnumerable<string>)?.SmartToString(Seperator);
        }

        #endregion
    }
}