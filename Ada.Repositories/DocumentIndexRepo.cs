namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ra.DomainEntities;
    using Ra.DomainEntities.Documents;

    #endregion

    public class DocumentIndexRepo : RepoBase, IAdaIngestRepository<DocIndexEntry>
    {
        #region  Constructors

        public DocumentIndexRepo(IAdaUowFactory adaUowFactory, int commitInterval) : base(adaUowFactory, commitInterval)
        {
        }

        #endregion

        #region IAdaIngestRepository<DocIndexEntry> Members

        public void SaveEntity(DocIndexEntry doc)
        {
            var commandText =
                new StringBuilder(
                        "INSERT INTO documents(dID, medieNumber, relativePath, materializedPath, pID, originalFileName, originalExtension, archiveExtension, gmlXSD, sessionKey) VALUES ('")
                    .Append(doc.DocumentId)
                    .Append("','")
                    .Append(doc.MediaId)
                    .Append("','")
                    .Append(doc.DocumentFolder)
                    .Append("','")
                    .Append($@"\Documents\{doc.DocumentFolder}\{doc.DocumentId}") // '' || B.relativePath || '\' || B.dID
                    .Append("',")
                    .Append(doc.ParentId ?? "NULL")
                    .Append(",'")
                    .Append(doc.OriginalFileName)
                    .Append("','")
                    .Append(doc.Extension)
                    .Append("','")
                    .Append(doc.SubmissionFileType)
                    .Append("',")
                    .Append(doc.GmlXsd == null ? "NULL" : string.Concat("'", doc.GmlXsd, "'"))
                    .Append(",'")
                    .Append(1)
                    .Append("')")
                    .ToString();

            ExecuteNonQuery(commandText);
        }

        //public void SaveEntity(DocIndexEntry entity)
        //{
        //    throw new NotImplementedException();
        //}

        public void Clear()
        {
            Clear<DocIndexEntry>();
        }

        #endregion

        #region

        public IEnumerable<DocIndexEntry> EnumerateDocs(FileTypeEnum fileType = FileTypeEnum.All)
        {
            // TODO filter out files not existing
            var CommandText = "SELECT dID, medieNumber, relativePath, pID, originalFileName, originalExtension, archiveExtension, gmlXSD, sessionKey FROM documents";
//            using (var cmd = this.Connection.CreateCommand())
//            {
//                cmd.CommandText = "SELECT dID, medieNumber, relativePath, pID, originalFileName, originalExtension, archiveExtension, gmlXSD, sessionKey FROM documents";

            if (fileType != FileTypeEnum.All)
            {
                var extension = fileType.GetExtension();
                CommandText += $" WHERE archiveExtension='{extension}' or archiveExtension='{extension.ToUpper()}' ;";
            }

            return EnumerateQuery(CommandText)
                .Select(
                    reader =>
                        new DocIndexEntry
                        {
                            DocumentId = reader["dID"].ToString(),
                            DocumentFolder = reader["relativePath"].ToString(),
                            GmlXsd = reader["gmlXSD"].ToString(),
                            MediaId = reader["medieNumber"].ToString(),
                            OriginalFileName = reader["originalFileName"].ToString(),
                            ParentId = reader["pID"].ToString(),
                            SubmissionFileType = reader["archiveExtension"].ToString()
                        });
        }

        public IEnumerable<string> GetDuplicateDocIds()
        {
            const string RecursiveIds = @"SELECT dID, COUNT(dID) FROM documents GROUP BY dID HAVING COUNT(dID)>1";

            return EnumerateQuery(RecursiveIds).Select(reader => reader["dID"].ToString());
        }

        public IEnumerable<string> GetRecursiveIds()
        {
            const string RecursiveIds = @"SELECT dID FROM recursiveIds";

            return EnumerateQuery(RecursiveIds).Select(reader => reader["dID"].ToString());
        }

        public long NestedDocumentCount()

        {
            var result = ExecuteScalarQuery("SELECT COUNT(pID) AS total FROM documents");

            return Convert.ToInt64(result);
        }


        public IEnumerable<(string dID, string pID)> NonExistentParentIds()
        {
            const string NonExistentParentIds = @"SELECT dID, pID FROM documents WHERE pID not IN(SELECT dID FROM documents)";

            return EnumerateQuery(NonExistentParentIds)
                .Select(reader => (reader["dID"].ToString(), reader["pID"].ToString()));
        }

//        public long TotalDocumentCount()
//        {
//            var result = ExecuteScalarQuery("SELECT COUNT(dID) AS total FROM documents");
//
//            return Convert.ToInt64(result);
//        }

        public long TotalDocumentCount(FileTypeEnum fileType = FileTypeEnum.All)
        {
            var CommandText = "SELECT COUNT(dID) AS total FROM documents";

            if (fileType != FileTypeEnum.All)
            {
                var extension = fileType.GetExtension();
                CommandText += $" WHERE archiveExtension='{extension}' or archiveExtension='{extension.ToUpper()}' ;";
            }


            var result = ExecuteScalarQuery(CommandText);

            return Convert.ToInt64(result);
        }

        #endregion
    }
}