namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexForeignKeyNotUnique : TableIndexDuplicateKeys
    {
        #region  Constructors

        public TableIndexForeignKeyNotUnique(ForeignKey fk)
            : base("6.C_7", fk)
        {
        }

        #endregion
    }
}