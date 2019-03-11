namespace Ada.Actions.Rewrite
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using ActionBase;
    using Checks;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using Log;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.FileIndex;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.FileIndex;
    using Ra.EntityExtensions.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("tableIndex", "files")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(FileIndexNotWellFormed), // '4.C.1_1'
        typeof(FileIndexInvalid), // '4.C.1_2'
        typeof(TableIndexNotWellFormed), // '4.C.1_7'
        typeof(TableIndexInvalid), // '4.C.1_8'
        typeof(SchemaMissingFolder), // '4.F_1'
        typeof(AdaAvSchemaVersionTableIndex), // '4.F_5'
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema), // '4.F_8'
        typeof(TableIndexDuplicateColumnName), // '6.C_2'
        typeof(TableIndexDuplicateColumnId), // '6.C_3'
        typeof(TableIndexReferencedTableMissing), // '6.C_19'
        typeof(TableIndexMissingParentColumns), // '6.C_20' 
        typeof(TableIndexForeignKeyColumnMissingInParent), // '6.C_22',
        typeof(TableIdentifierReservedWords), // '6.C_28'
        typeof(TableNameDuplicate),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    public class XsdRewriteAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        // TODO remove hack for clear of rewrite
        private readonly IAdaProcessLog processLog;

        #endregion

        #region  Constructors

        public XsdRewriteAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.processLog = processLog;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var testuow = (UnitOfWork) factory.GetUnitOfWork())
            {
                testuow.BeginTransaction();
                foreach (var table in testuow.GetRepository<Table>().All().AsParallel())
                {
                    var fileRepo = testuow.GetRepository<FileIndexEntry>();
                    var xsdMetaData = fileRepo.All()
                        .FirstOrDefault(x => x.Extension == ".xsd" && x.FileName == table.Folder);

                    if (xsdMetaData == null)
                        continue;

                    var md5 = MD5.Create();
                    using (var memStream = table.GetXmlSchemaStream())
                    {
                        var hash = BitConverter.ToString(md5.ComputeHash(memStream)).Replace("-", string.Empty);

                        //if (hash != xsdMetaData.Md5) //TODO: only rewrite if existing xsd doesn't match generated
                        //{
                        var path = Mapping.GetMediaPath(int.Parse(xsdMetaData.MediaNumber))
                                   + xsdMetaData.RelativePathAndFile();
                        using (var fileStream = File.Create(path))
                        {
                            memStream.Position = 0;
                            memStream.CopyTo(fileStream);
                            fileStream.Close();
                            memStream.Close();
                            xsdMetaData.Md5 = hash;
                            fileRepo.Update(xsdMetaData);
                        }
                    }

                    //}
                }

                testuow.Commit();
            }
        }

        public ActionRunResult ResetAndRun(IAdaUowFactory factory)
        {
            var previousProcessLogEntry = processLog.GetExistingProcessLog(GetId(factory));

            if (previousProcessLogEntry != null)
                Clear(previousProcessLogEntry);

            return Run(factory);
        }

        #endregion
    }
}