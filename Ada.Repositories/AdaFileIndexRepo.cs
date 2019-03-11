namespace Ada.Repositories
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ra.DomainEntities;
    using Ra.DomainEntities.FileIndex;

    #endregion

    public class AdaFileIndexRepo : RepoBase, IAdaIngestRepository<FileIndexEntry>
    {
        #region  Constructors

        public AdaFileIndexRepo(IAdaUowFactory adaUowFactory, int commitInterval) : base(adaUowFactory, commitInterval)
        {
        }

        #endregion

        #region IAdaIngestRepository<FileIndexEntry> Members

        public void SaveEntity(FileIndexEntry file)
        {
            var commandText =
                new StringBuilder(
                        "INSERT INTO files(mediaNumber, relativePath, [fileName], extension, md5, [foN]) VALUES ('")
                    .Append(file.MediaNumber)
                    .Append("','")
                    .Append(file.RelativePath)
                    .Append("','")
                    .Append(file.FileName)
                    .Append("','")
                    .Append(file.Extension)
                    .Append("','")
                    .Append(file.Md5)
                    .Append("','")
                    .Append(file.foN)
                    .Append("')")
                    .ToString();

            ExecuteNonQuery(commandText);
        }

        public void Clear()
        {
            Clear<FileIndexEntry>();
        }

        #endregion

        #region

        public IEnumerable<FileIndexEntry> EnumerateFiles(FileTypeEnum fileType = FileTypeEnum.All)
        {
            var CommandText = "SELECT fileName, mediaNumber, extension, relativePath, md5, [foN], timeStamp FROM files";

            if (fileType != FileTypeEnum.All)
            {
                var extension = fileType.GetExtension();
                CommandText += $" WHERE extension='{extension}' or extension='{extension.ToUpper()}' ;";
            }

            return EnumerateQuery(CommandText).Select(reader =>
                new FileIndexEntry
                {
                    MediaNumber = reader["mediaNumber"].ToString(),
                    RelativePath = reader["relativePath"].ToString(),
                    FileName = reader["fileName"].ToString(),
                    Extension = reader["extension"].ToString(),
                    Md5 = reader["md5"].ToString(),
                    foN = reader["foN"].ToString()
                }
            );
        }

        #endregion
    }
}