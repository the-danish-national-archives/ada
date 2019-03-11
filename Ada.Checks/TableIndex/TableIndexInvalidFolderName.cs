namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class TableIndexInvalidFolderName : AdaAvViolation
    {
        #region  Constructors

        public TableIndexInvalidFolderName(Table table)
            : base("6.C_27")
        {
            TableName = table.Name;
            FolderName = table.Folder;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string FolderName { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table)
        {
            if (!table.FolderNameIsValid()) yield return new TableIndexInvalidFolderName(table);
        }

        #endregion
    }
}