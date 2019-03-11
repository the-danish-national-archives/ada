namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using Log;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.ArchiveIndex;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("TableIndex", "ArchiveIndex")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3' " // 
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1' " // 
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2' " // 
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3' " // 
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1' " // 
        typeof(FolderStructureSchemasMissing), // '4.B.2_2' " // 
        typeof(FolderStructureTablesMissing), // '4.B.2_4' " // 
        typeof(IndicesFileIndex), // '4.C_1' " // 
        typeof(IndicesTableIndex),
        typeof(FileIndexNotWellFormed), // '4.C.1_1' " // 
        typeof(FileIndexInvalid), // '4.C.1_2' " // 
        typeof(TableIndexNotWellFormed),
        typeof(TableIndexInvalid), // '4.C.1_8' " // 
        typeof(TableNameDuplicate), // '4.C.5_3' " // 
        typeof(SchemaMissingFolder), // '4.F_1' " // 
        typeof(AdaAvSchemaVersionTableIndex), // '4.F_5' " // 
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6' " // 
        typeof(AdaAvSchemaVersionXmlSchema), // '4.F_8' " // 
        typeof(TableIndexDuplicateColumnName), // '6.C_2' " // 
        typeof(TableIndexReferencedTableMissing), // '6.C_19' " // 
        typeof(TableIndexMissingParentColumns), // '6.C_20' " // 
        typeof(TableIdentifierReservedWords),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(
        typeof(TableIndexFunctionalDescriptionListing),
        typeof(ArchiveIndexDocTitleMissing),
        typeof(ArchiveIndexDocDateMissing),
        typeof(ArchiveIndexHaveDigitalDocsNoContainingDocsMark),
        typeof(ArchiveIndexMarkedContainingDocIdWithNoDigitalDocs),
        typeof(TableIndexContainsCaseId),
        typeof(ArchiveIndexCaseIdOrCaseTitleMissing)
    )]
    public class FunctionalDescriptionTestAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public FunctionalDescriptionTestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            List<FunctionalDescription> functionalDescriptionsUsed;
            bool containsDigitalDocuments;
            bool systemFileConcept;

            ILookup<FunctionalDescription, Column> colFuncDesc = null;

            using (var uow = (UnitOfWork) factory.GetUnitOfWork())
            {
                colFuncDesc = uow.GetRepository<Column>()
                    .FilterBy(x => x.FunctionalDescriptions.Any()).AsEnumerable()
                    .SelectMany(c => c.FunctionalDescriptions.Select(fd => (Fd: fd, Col: c)))
                    .ToLookup(t => t.Fd, t => t.Col);
                functionalDescriptionsUsed = colFuncDesc.Select(g => g.Key).ToList();

                //                    uow.GetRepository<Column>()
                //                        .FilterBy(x => x.FunctionalDescriptions.Any())
                //                        .SelectMany(x => x.FunctionalDescriptions)
                //                        .ToList();


                var archiveIndex = uow.GetRepository<ArchiveIndex>().All().FirstOrDefault();
                containsDigitalDocuments = archiveIndex?.ContainsDigitalDocuments ?? false;
                systemFileConcept = archiveIndex?.SystemFileConcept ?? false;

                ReportAny(TableIndexFunctionalDescriptionListing.Create(colFuncDesc));
            }


            if (containsDigitalDocuments)
            {
                ReportAny(ArchiveIndexDocTitleMissing.Check(functionalDescriptionsUsed));
                ReportAny(ArchiveIndexDocDateMissing.Check(functionalDescriptionsUsed));
                ReportAny(ArchiveIndexHaveDigitalDocsNoContainingDocsMark.Check(functionalDescriptionsUsed));
            }
            else
            {
                ReportAny(ArchiveIndexMarkedContainingDocIdWithNoDigitalDocs.Check(functionalDescriptionsUsed));
            }

            if (!systemFileConcept)
                ReportAny(TableIndexContainsCaseId.Check(functionalDescriptionsUsed));
            else
                ReportAny(ArchiveIndexCaseIdOrCaseTitleMissing.Check(functionalDescriptionsUsed));
        }

        #endregion
    }
}