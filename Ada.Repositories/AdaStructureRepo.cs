namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ra.DomainEntities.FileSystem;

    #endregion

    public class AdaStructureRepo : RepoBase, IAdaIngestRepository<FileSystemFolder>
    {
        #region  Fields

        private bool disposed;

        private Dictionary<string, string> keys = new Dictionary<string, string>();

        #endregion

        #region  Constructors

        public AdaStructureRepo(IAdaUowFactory adaUowFactory, int commitInterval)
            : base(adaUowFactory, commitInterval)
        {
        }

        #endregion

        #region IAdaIngestRepository<FileSystemFolder> Members

        public void SaveEntity(FileSystemFolder entry)
        {
            var entryKey = GetKey(entry);
            if (entryKey == null)
                entryKey = InsertFolder(entry);
            else
                UpdateFolder(entry, entryKey);

            var ancestors = new List<string>();
            foreach (var parent in entry.Ancestors) ancestors.Add(GetKey(parent));

            ancestors.Add(entryKey);

            foreach (var child in entry.Contents)
            {
                string childKey = null;
                var folder = child as FileSystemFolder;
                if (folder != null)
                    childKey = InsertFolder(folder);

                var file = child as FileSystemFile;
                if (file != null)
                    childKey = InsertFile(file);

                foreach (var ancestor in ancestors) UpdateClosure(ancestor, childKey);
            }
        }

        public void Clear()
        {
            Clear<FileSystemFolder>();
        }

        #endregion

        #region

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) keys = null;

                disposed = true;
            }

            base.Dispose(disposing);
        }

        private string EntryToKeyId(FileSystemEntry entry)
        {
            return new StringBuilder()
                .Append(entry.RelativePath)
                .Append("' AND name='")
                .Append(entry.Name)
                .Append("' AND mediaNumber=")
                .Append(entry.MediaNumber)
                .ToString();
        }


        public IEnumerable<FileSystemFile> EnumerateDocFilesOrder(bool forContextDocmunetation)
        {
            var CommandText =
                $@"SELECT mediaNumber, entryKey, entryType, relativePath, name, dID, extension, size, [checksum]
FROM
    {(forContextDocmunetation
                    ? "contextDocumentationDocumentContentOnDisk"
                    : "documentContentOnDisk")}
    ORDER BY relativePath, name";

            return EnumerateQuery(CommandText).Select(reader =>
                new FileSystemFile
                {
                    Key = Convert.ToInt32(reader["entryKey"]),
                    MediaNumber = Convert.ToInt32(reader["mediaNumber"].ToString()),
                    RelativePath = reader["relativePath"].ToString(),
                    Name = reader["name"].ToString(),
                    Extension = reader["extension"].ToString()
                });
        }

        public IEnumerable<FileSystemFile> EnumerateFiles()
        {
            var CommandText =
                "SELECT entryKey, mediaNumber, relativePath, name, extension, [timestamp] FROM fsEntries WHERE entryType='File'";

            return
                EnumerateQuery(CommandText)
                    .Select(
                        reader =>
                            new FileSystemFile
                            {
                                Key = Convert.ToInt32(reader["entryKey"]),
                                MediaNumber = Convert.ToInt32(reader["mediaNumber"].ToString()),
                                RelativePath = reader["relativePath"].ToString(),
                                Name = reader["name"].ToString(),
                                Extension = reader["extension"].ToString()
                            });
        }

        public IEnumerable<FileSystemFile> EnumerateTableFiles()
        {
            var CommandText =
                "SELECT mediaNumber, entryKey, relativePath, name, extension FROM tablesOnDisk WHERE entryType='File'";

            return
                EnumerateQuery(CommandText)
                    .Select(
                        reader =>
                            new FileSystemFile
                            {
                                Key = Convert.ToInt32(reader["entryKey"]),
                                MediaNumber = Convert.ToInt32(reader["mediaNumber"].ToString()),
                                RelativePath = reader["relativePath"].ToString(),
                                Name = reader["name"].ToString(),
                                Extension = reader["extension"].ToString()
                            });
        }

        protected string ExecuteWithLastInsertRowId(string command)
        {
            const string GetLastRowInsertCommandText = "; SELECT last_insert_rowid();";
            return ExecuteScalarQuery(command + GetLastRowInsertCommandText, true)?.ToString();
        }

        protected string GetKey
        (
            FileSystemFolder entry)
        {
            string result;
            keys.TryGetValue(EntryToKeyId(entry), out result);
            return result;
        }

        public FileSystemFile GetStandardSchema(string name)
        {
            var commandText =
                $"SELECT entryKey, relativePath, name, extension FROM standardSchemasOnDisk WHERE name='{name}.xsd'";

            FileSystemFile res = null;

            // only one is expected, but EnumerateQuery is needed to run through
            foreach (var file in EnumerateQuery(commandText)
                .Select(
                    reader =>
                        new FileSystemFile
                        {
                            Key = Convert.ToInt32(reader["entryKey"]),
                            RelativePath = reader["relativePath"].ToString(),
                            Name = reader["name"].ToString(),
                            Extension = reader["extension"].ToString()
                        }))
                res = file;

            return res;
        }

        public IEnumerable<string> GetTableList()
        {
            var tableFolderSelectQuery = "SELECT tableNumber FROM tablesOnDisk WHERE extension = '.xml' COLLATE NOCASE";

            return EnumerateQuery(tableFolderSelectQuery).Select(reader => reader.GetString(0));
        }

        public bool HasDocuments()
        {
            var CommandText = "SELECT COUNT(*) FROM mediaWithDocuments";
            var obj = ExecuteScalarQuery(CommandText);
            return Convert.ToInt32(obj) > 0;
        }

        protected string InsertFile(FileSystemFile file)
        {
            const string InsertFileTemplate =
                "INSERT INTO fsEntries (extension, size, relativePath, name, depth, timestamp, mediaNumber, entryType) VALUES ('";
            var insertFileCommandText =
                new StringBuilder(InsertFileTemplate)
                    .Append(file.Extension)
                    .Append("','")
                    .Append(file.Size)
                    .Append("','")
                    .Append(file.RelativePath)
                    .Append("','")
                    .Append(file.Name)
                    .Append("','")
                    .Append(file.Level)
                    .Append("','")
                    .Append(file.TimeStamp)
                    .Append("','")
                    .Append(file.MediaNumber)
                    .Append("','")
                    .Append("File")
                    .Append("')")
                    .ToString();

            var childKey = ExecuteWithLastInsertRowId(insertFileCommandText);
            return childKey;
        }

        protected string InsertFolder(FileSystemFolder folder)
        {
            const string InsertFolderTemplate =
                "INSERT INTO fsEntries (fileCount, folderCount, relativePath, name, depth, timestamp, mediaNumber, entryType) VALUES ('";
            var insertFolderCommandText =
                new StringBuilder(InsertFolderTemplate)
                    .Append(folder.FileCount)
                    .Append("','")
                    .Append(folder.FolderCount)
                    .Append("','")
                    .Append(folder.RelativePath)
                    .Append("','")
                    .Append(folder.Name)
                    .Append("','")
                    .Append(folder.Level)
                    .Append("','")
                    .Append(folder.TimeStamp)
                    .Append("','")
                    .Append(folder.MediaNumber)
                    .Append("','")
                    .Append("Dir")
                    .Append("')")
                    .ToString();

            var childKey = ExecuteWithLastInsertRowId(insertFolderCommandText);

            keys.Add(EntryToKeyId(folder), childKey);
            return childKey;
        }

        public bool MarkedWithDocuments()
        {
            var CommandText = "SELECT COUNT(*) FROM archiveIndex WHERE containsDigitalDocuments='1'";
            var obj = ExecuteScalarQuery(CommandText);
            return Convert.ToInt32(obj) > 0;
        }

        public long TotalFiles()
        {
            var CommandText =
                "SELECT count(*) FROM fsEntries WHERE entryType='File'";

            return Convert.ToInt64(ExecuteScalarQuery(CommandText));
        }

        protected void UpdateClosure(string ancestor, string descendant)
        {
            const string insertClosureTemplate = "INSERT INTO fsEntryClosure (ancestor, descendant) VALUES ('";
            var insertClosureCommandText = new StringBuilder(insertClosureTemplate)
                .Append(ancestor)
                .Append("','")
                .Append(descendant)
                .Append("')")
                .ToString();
            ExecuteNonQuery(insertClosureCommandText);
        }

        protected void UpdateFolder(FileSystemFolder folder, string entryKey)
        {
            const string InsertFolderTemplate = "Update fsEntries SET fileCount='";
            var insertFolderCommandText =
                new StringBuilder(InsertFolderTemplate)
                    .Append(folder.FileCount)
                    .Append("', folderCount='")
                    .Append(folder.FolderCount)
                    .Append("' WHERE entryKey=")
                    .Append(entryKey)
                    .ToString();

            ExecuteNonQuery(insertFolderCommandText);
        }

        public void UpdateMD5(FileSystemFile file)
        {
            var commandText =
                new StringBuilder("UPDATE fsEntries SET checkSum = '").Append(file.CheckSum)
                    .Append("' WHERE entryKey = ")
                    .Append(file.Key)
                    .ToString();
            ExecuteNonQuery(commandText);
        }

        #endregion
    }
}