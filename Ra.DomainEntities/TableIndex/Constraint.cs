namespace Ra.DomainEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Shared superclass for PrimaryKey and ForeignKey
    /// </summary>
    public abstract class Constraint : EntityBase
    {
        #region Properties

        /// <summary>
        ///     Used when inherited as a Primary key
        /// </summary>
        public virtual IList<ConstraintColumn> ConstraintColumns { get; set; }

        /// <summary>
        ///     Name of the foreign key according to SQL:1999 rules for SQL Identifiers
        /// </summary>
        public virtual string Name { get; set; }
//        public virtual string ConstraintType { get; set; }

        /// <summary>
        ///     Backward reference, generated (might be leasy loaded)
        /// </summary>
        public virtual Table ParentTable { get; set; }

        /// <summary>
        ///     Backward reference, generated (same as ParentTable.Name, but already loaded)
        /// </summary>
        public virtual string ParentTableName { get; set; }

        #endregion
    }
}