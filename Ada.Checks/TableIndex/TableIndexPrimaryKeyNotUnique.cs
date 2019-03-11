namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexPrimaryKeyNotUnique : TableIndexDuplicateKeys
    {
        #region  Constructors

        public TableIndexPrimaryKeyNotUnique(PrimaryKey pk)
            : base("6.C_6", pk)
        {
        }

        #endregion
    }
}