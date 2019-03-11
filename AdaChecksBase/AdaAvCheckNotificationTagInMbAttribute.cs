namespace Ada.ChecksBase
{
    #region Namespace Using

    using System.Globalization;

    #endregion

    public class AdaAvCheckNotificationTagInMbAttribute : AdaAvCheckNotificationTagAttribute
    {
        #region

        public override string ValueToString(object o)
        {
            return (o as double?)?.ToString("N3", CultureInfo.InvariantCulture) ?? "";
        }

        #endregion
    }
}