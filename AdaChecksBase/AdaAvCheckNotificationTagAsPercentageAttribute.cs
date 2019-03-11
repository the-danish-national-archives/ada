namespace Ada.ChecksBase
{
    #region Namespace Using

    using System.Globalization;

    #endregion

    public class AdaAvCheckNotificationTagAsPercentageAttribute : AdaAvCheckNotificationTagAttribute
    {
        #region

        public override string ValueToString(object o)
        {
            return (o as float?)?.ToString("0.00", CultureInfo.InvariantCulture);
        }

        #endregion
    }
}