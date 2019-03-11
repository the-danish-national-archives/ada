namespace Ra.DomainEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion


    public class PrimaryKey : Constraint
    {
        #region Properties

        public virtual IList<string> Columns { get; set; }

        #endregion
    }
}