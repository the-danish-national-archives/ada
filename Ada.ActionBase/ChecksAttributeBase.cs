namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;

    #endregion

    public class ChecksAttributeBase : Attribute
    {
        #region  Constructors

        public ChecksAttributeBase(params Type[] check)
        {
            if (check.Any(c => !c.IsSubclassOf(typeof(AdaAvCheckNotification))))
                throw new ArgumentException(nameof(check), "Most be of array type AdaAvCheckNotification");
            Checks = check;
        }

        #endregion

        #region Properties

        public IEnumerable<Type> Checks { get; }

        #endregion
    }
}