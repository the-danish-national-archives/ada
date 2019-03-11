namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableFolderSequenceInvalidStart : AdaAvViolation
    {
        #region  Constructors

        public TableFolderSequenceInvalidStart()
            : base("4.D_4")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(TableIndex index)
        {
            if (!index.Tables.Any(x => x.Folder.Equals("table1"))) yield return new TableFolderSequenceInvalidStart();
        }

        #endregion
    }
}