namespace Ra.DomainEntities.TableIndex
{
    /// <summary>
    ///     Column that are part of a primary key
    /// </summary>
    public class ConstraintColumn : EntityBase
    {
        #region Properties

        /// <summary>
        ///     Column name, one of the columns in the table of the primarykey
        /// </summary>
        public virtual string Column { get; set; }

        /// <summary>
        ///     References parent (primarykey)
        /// </summary>
        public virtual Constraint Constraint { get; set; }

        /// <summary>
        ///     Name of parent (primarykey)
        /// </summary>
        public virtual string ConstraintName { get; set; }

        /// <summary>
        ///     Name of the table the primarykey belongs to
        /// </summary>
        public virtual string TableName { get; set; }

        #endregion
    }
}