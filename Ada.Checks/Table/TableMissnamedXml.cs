namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Repositories;

    #endregion

    public class TableMissnamedXml : AdaAvViolation
    {
        #region  Constructors

        public TableMissnamedXml(string path)
            : base("4.D_7")
        {
            Path = path;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(AdaStructureRepo repo)
        {
            foreach (var file in repo.EnumerateTableFiles())
            {
                if (file.Extension.ToLower() != ".xml")
                    continue;

                if (file.RelativePath.Split('\\').LastOrDefault() == file.Name.Substring(0, file.Name.Length - 4))
                    continue;

                yield return new TableMissnamedXml(file.RelativePath);
            }
        }

        #endregion
    }
}