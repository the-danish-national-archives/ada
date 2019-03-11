namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.DocIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using Log;
    using Repositories;

    #endregion

    [AdaActionPrecondition("DocumentsIndex", "Tables")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), //'4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), //'4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), //'4.B.2_1'
        typeof(FolderStructureSchemasMissing), //'4.B.2_2'
        typeof(FolderStructureTablesMissing), //'4.B.2_4'
        typeof(IndicesFileIndex), //'4.C_1' 
        typeof(IndicesTableIndex), //'4.C_4' 
        typeof(IndicesDocIndex), //'4.C_5' 
        typeof(FileIndexNotWellFormed), //'4.C.1_1' 
        typeof(FileIndexInvalid), //'4.C.1_2' 
        typeof(TableIndexNotWellFormed), //'4.C.1_7' 
        typeof(TableIndexInvalid), //'4.C.1_8' 
        typeof(DocIndexNotWellFormed), //'4.C.1_9' 
        typeof(DocIndexInvalid), //'4.C.1_9' 
        typeof(TableNameDuplicate), //'4.C.5_3' 
        typeof(TableTestMissingTable), //'4.D_1'  
        typeof(TableTestNotInIndex), //'4.D_2'
        typeof(TableMissnamedXml), //'4.D_7' 
        typeof(TableNotWellFormed), //'4.D_8' 
        typeof(TableNotValid), //'4.D_9' 
        typeof(SchemaMissingFolder), //'4.F_1' 
        typeof(AdaAvSchemaVersionTableIndex), //'4.F_5' 
        typeof(AdaAvSchemaVersionDocIndex), //'4.F_7' 
        typeof(AdaAvSchemaVersionFileIndex), //'4.F_6' 
        typeof(AdaAvSchemaVersionXmlSchema), //'4.F_8' 
        typeof(TableIndexDuplicateColumnName), //'6.C_2' 
        typeof(TableIndexDuplicateColumnId), //'6.C_3'
        typeof(ArchiveIndexHaveDigitalDocsNoContainingDocsMark), //'6.C_14_1 
        typeof(TableIndexReferencedTableMissing), //'6.C_19' 
        typeof(TableIndexMissingParentColumns), //'6.C_20' 
        typeof(TableIndexForeignKeyColumnMissingInReferenced), //'6.C_21' 
        typeof(TableIndexForeignKeyColumnMissingInParent),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(TableIdentifierReservedWords),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(
        typeof(DocIndexDocsNotRefereencedFromTables),
        typeof(DocIndexDocsOnlyRefereencedViaParentFromTables),
        typeof(DocIndexDocsReferedFromTablesNotExisting)
    )]
    public class DocumentRelationTestAction : AdaActionBase<FullDocumentRepo>
    {
        #region  Constructors

        public DocumentRelationTestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(FullDocumentRepo fullDocRepo)
        {
            var docIdCount = fullDocRepo.DocIdCountFromDocIndex();
            var p = fullDocRepo.DocIdOrphansFromDocIndex();


            ReportAny(DocIndexDocsNotRefereencedFromTables.Check(docIdCount, p.notEvenViaParent));
            ReportAny(DocIndexDocsOnlyRefereencedViaParentFromTables.Check(docIdCount, p.onlyViaParent));
            ReportAny(DocIndexDocsReferedFromTablesNotExisting.Check(fullDocRepo));
        }

        #endregion
    }
}