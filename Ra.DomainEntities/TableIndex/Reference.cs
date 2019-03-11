namespace Ra.DomainEntities.TableIndex
{
    /// <summary>
    /// reference element in siardDiark Archive
    /// </summary>
    public class Reference : EntityBase
    {
        /// <summary>
        /// Reference to owning foreingkey
        /// </summary>
        public virtual ForeignKey ParentConstraint { get; set; }

        /// <summary>
        /// Referencing column
        /// </summary>
        public virtual string Column { get; set; }

        /// <summary>
        /// Referenced column <br/>
        /// Oprindeligt navn på de kolonner, fremmednøglen refererer til
        /// </summary>
        public virtual string Referenced { get; set; }
    }
}