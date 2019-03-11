namespace Ra.DomainEntities.TableIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class TableIndex : EntityBase
    {
        #region  Constructors

        public TableIndex()
        {
            Version = "1.0";
        }

        #endregion

        #region Properties

        public virtual string DbName { get; set; }

        public virtual string DbProduct { get; set; }

        public virtual Guid SessionKey { get; set; }

        public virtual IList<Table> Tables { get; set; }

        public virtual string Version { get; set; }

        public virtual IList<View> Views { get; set; }

        #endregion
    }
}