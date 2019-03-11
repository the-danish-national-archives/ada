namespace Ada.ActionBase
{
    #region Namespace Using

    using System;

    #endregion

    public class RequiredChecksAttribute : ChecksAttributeBase
    {
        #region  Constructors

        public RequiredChecksAttribute(params Type[] check)
            : base(check)
        {
        }

        #endregion
    }
}