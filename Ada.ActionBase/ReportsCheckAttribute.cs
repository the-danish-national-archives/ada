namespace Ada.ActionBase
{
    #region Namespace Using

    using System;

    #endregion

    public class ReportsChecksAttribute : ChecksAttributeBase
    {
        #region  Constructors

        public ReportsChecksAttribute(params Type[] check)
            : base(check)
        {
        }

        #endregion
    }
}