namespace Ra.DomainEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public class Table : EntityBase, IAnnotatedEntity
    {
        #region Properties

        public virtual IList<Column> Columns { get; set; }

        public virtual string Folder { get; set; }

        public virtual IList<ForeignKey> ForeignKeys { get; set; }

        public virtual string Name { get; set; }

        public virtual PrimaryKey PrimaryKey { get; set; }

        public virtual string Rows { get; set; }
        public virtual TableIndex TableIndex { get; set; }

        #endregion

        #region IAnnotatedEntity Members

        public virtual string Description { get; set; }

        #endregion
    }
}