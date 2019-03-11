namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;
    using Ra.DomainEntities.Documents;
    using Ra.DomainEntities.FileIndex;

    #endregion

    public class FullDocumentRepoHacker : IDisposable // : BaseRepo
    {
        #region  Fields

        private readonly AViD _aviD;

        private readonly string _dbCreationFolder;

        private readonly IAdaUowFactory _testFactory;

        #endregion

        #region  Constructors

        public FullDocumentRepoHacker
        (IAdaUowFactory testFactory, AViD aviD
            , string dbCreationFolder, bool forContextDocmunetation = false
        )
        {
            _testFactory = testFactory;
            _aviD = aviD;
            _dbCreationFolder = dbCreationFolder;
            IsForContextDocmunetation = forContextDocmunetation;
        }

        #endregion

        #region Properties

        public bool IsForContextDocmunetation { get; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // nothing
            // done in EnumerateQuery for now
        }

        #endregion

        #region

        public long DocFilesCount
        (
            FileTypeEnum fileType = FileTypeEnum.All)
        {
            var CommandText = $@"SELECT COUNT(*) FROM files as A JOIN
            {
                (IsForContextDocmunetation
                    ? "contextDocumentationDocuments"
                    : "documents")} as B ";

            if (IsForContextDocmunetation)
                CommandText += " WHERE " +
                               @"A.relativePath == B.materializedPath";
            else
                CommandText += " WHERE A.mediaNumber == B.medieNumber AND " +
                               @"A.relativePath == B.materializedPath";

            if (fileType != FileTypeEnum.All)
            {
                var extension = fileType.GetExtension();
                if (IsForContextDocmunetation)
                    CommandText += $" AND UPPER(A.extension)=='.{extension.ToUpper()}' ;";
                else
                    CommandText += $" AND UPPER(B.archiveExtension)=='{extension.ToUpper()}' ;";
            }

            var res = EnumerateQuery(CommandText).Select(reader => long.TryParse(reader[0].ToString(), out var i) ? i : 0).LastOrDefault();
            return res;
        }

        public IEnumerable<(DocIndexEntry doc, FileIndexEntry file)> EnumerateDocFiles
        (
            FileTypeEnum fileType = FileTypeEnum.All)
        {
            var CommandText =
                @"SELECT A.fileKey  as AfileKey , A.mediaNumber  as AmediaNumber , 
                    A.relativePath  as ArelativePath , A.[fileName] as AfileName, A.extension  as Aextension , 
                    A.md5  as Amd5 ,  A.foN as AfoN , B.dID as BdID, B.medieNumber as BmedieNumber, 
                    B.relativePath as BrelativePath, B.originalFileName as BoriginalFileName, 
                    B.archiveExtension as BarchiveExtension,"
                + $@"
                    {(IsForContextDocmunetation
                    ? @""
                    : @"B.pID as BpID, B.originalExtension as BoriginalExtension, B.gmlXSD as BgmlXSD,")}
                    B.sessionKey as BsessionKey "
                + $@"
                FROM files as A JOIN 
                    {(IsForContextDocmunetation
                    ? "contextDocumentationDocuments"
                    : "documents")} as B ";

            if (IsForContextDocmunetation)
                CommandText += " WHERE " +
                               @"A.relativePath == B.materializedPath";
            else
                CommandText += " WHERE A.mediaNumber == B.medieNumber AND " +
                               @"A.relativePath == B.materializedPath";

            if (fileType != FileTypeEnum.All)
            {
                var extension = fileType.GetExtension();
                if (IsForContextDocmunetation)
                    CommandText += $" AND UPPER(A.extension)=='.{extension.ToUpper()}' ;";
                else
                    CommandText += $" AND UPPER(B.archiveExtension)=='{extension.ToUpper()}' ;";
            }

            return EnumerateQuery(CommandText).Select(
                reader => (
                    new DocIndexEntry
                    {
                        DocumentId = reader["BdID"].ToString(),
                        DocumentFolder = reader["BrelativePath"].ToString(),
                        GmlXsd = IsForContextDocmunetation ? "" : reader["BgmlXSD"].ToString(),
                        MediaId = reader["BmedieNumber"].ToString(),
                        OriginalFileName = reader["BoriginalFileName"].ToString(),
                        ParentId = IsForContextDocmunetation ? "" : reader["BpID"].ToString(),
                        SubmissionFileType = reader["BarchiveExtension"].ToString()
                    },
                    new FileIndexEntry
                    {
                        MediaNumber = reader["AmediaNumber"].ToString(),
                        RelativePath = reader["ArelativePath"].ToString(),
                        FileName = reader["AfileName"].ToString(),
                        Extension = reader["Aextension"].ToString(),
                        Md5 = reader["Amd5"].ToString(),
                        foN = reader["AfoN"].ToString()
//                                      TimeStamp = reader["AtimeStamp"].ToString(),
                    })
            );
        }

        public IEnumerable<IDataReader> EnumerateQuery(string query)
        {
            using (var uow = (UnitOfWork) _testFactory.GetUnitOfWork())
            {
                var _connection = uow.Session.Connection;
                var cmdAttach = _connection.CreateCommand();

                var path = Path.Combine(_dbCreationFolder, _aviD.FullID + ".av");

                cmdAttach.CommandText = "ATTACH '" + path + "' AS testDB";
                cmdAttach.ExecuteNonQuery();

                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = query;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) yield return reader;
                    }
                }
            }
        }

        public override string ToString()
        {
            return GetType().FullName + $"+{nameof(IsForContextDocmunetation)}:{IsForContextDocmunetation}";
        }

        #endregion
    }
}