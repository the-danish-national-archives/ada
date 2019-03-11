namespace Ada.ActionBase
{
    #region Namespace Using

    using System;

    #endregion

    public class AdaActionPreconditionAttribute : Attribute
    {
        #region  Constructors

        public AdaActionPreconditionAttribute(params string[] conditions)
        {
            Conditions = conditions;
        }

        #endregion

        #region Properties

        public string[] Conditions { get; set; }

        #endregion
    }
}