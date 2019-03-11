namespace Ra.EntityExtensions.TableIndex
{
    #region Namespace Using

    using System.Linq;
    using DomainEntities.TableIndex;

    #endregion

    public static class ForeignKeyExtensions
    {
        #region

        public static Table GetReferencedTable(this ForeignKey fk)
        {
            return fk.ParentTable.TableIndex.Tables.FirstOrDefault(x => x.Name.Equals(fk.ReferencedTable));
        }

        #endregion
    }
}