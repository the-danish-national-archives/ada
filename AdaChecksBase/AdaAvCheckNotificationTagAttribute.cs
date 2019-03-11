namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Property)]
    public class AdaAvCheckNotificationTagAttribute : Attribute
    {
        #region  Fields

        public bool Hidden = false;

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion

        #region

        public virtual string ValueToString(object o)
        {
            return o.ToString();
        }

        #endregion
    }
}