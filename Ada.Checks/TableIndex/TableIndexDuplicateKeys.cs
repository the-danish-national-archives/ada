namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexDuplicateKeys : AdaAvViolation
    {
        #region  Constructors

        public TableIndexDuplicateKeys(string tagType, Constraint ck)
            : base(tagType)
        {
            ConstraintName = ck.Name;
            TableName = ck.ParentTableName;
            FolderName = ck.ParentTable.Folder;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ConstraintName { get; set; }

        [AdaAvCheckNotificationTag]
        public string FolderName { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(TableIndex index)
        {
            foreach (
                var keyDuplicate in
                index.Tables.SelectMany(t => t.PrimaryKey.Yield<Constraint>().Union(t.ForeignKeys))
                    .GroupBy(x => x.Name)
                    .Where(g => g.Skip(1).Any())
                    .SelectMany(g => g)
            )
            {
                if (keyDuplicate is ForeignKey)
                    yield return new TableIndexForeignKeyNotUnique(keyDuplicate as ForeignKey);

                if (keyDuplicate is PrimaryKey)
                    yield return new TableIndexPrimaryKeyNotUnique(keyDuplicate as PrimaryKey);
            }
        }

        #endregion
    }
}