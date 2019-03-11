namespace Ra.DomainEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    public class Column : EntityBase
    {
        #region Properties

        public virtual string ColumnId { get; set; }

        public virtual string DefaultValue { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<FunctionalDescription> FunctionalDescriptions { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Nullable { get; set; }
        public virtual Table ParentTable { get; set; }

        public virtual string TableName { get; set; }

        public virtual string Type { get; set; }

        public virtual string TypeOriginal { get; set; }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var that = (Column) obj;
            return Name == that.Name && ParentTable == that.ParentTable;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ ParentTable.GetHashCode();
        }

        #endregion
    }
}