namespace Ada.ADA.Common.TestActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.ExtensionMethods;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.ArchiveIndex;
    using Ra.DomainEntities.TableIndex;

    public class FunctionalDescriptionTest : AdaActionBase<IAdaUowFactory>
    {

        protected override void OnRun(IAdaUowFactory factory)
        {
            List<FunctionalDescription> functionalDescriptions;
            var functionalDescriptionUsageList = new List<string>();
            bool containsDigitalDocuments;
            bool systemFileConcept;

            using (var uow = (UnitOfWork)factory.GetUnitOfWork())
            {
                functionalDescriptions =
                    uow.GetRepository<Column>()
                        .FilterBy(x => x.FunctionalDescriptions.Any())
                        .SelectMany(x => x.FunctionalDescriptions)
                        .ToList();
            
                foreach (FunctionalDescription func in Enum.GetValues(typeof(FunctionalDescription)))
                {
                    functionalDescriptionUsageList.Add(
                        string.Concat(func.ToString(), ":", functionalDescriptions.Count(x => x == func).ToString()));
                }

                var archiveIndex = uow.GetRepository<ArchiveIndex>().All().FirstOrDefault();
                containsDigitalDocuments = archiveIndex?.ContainsDigitalDocuments ?? false; 
                systemFileConcept = archiveIndex?.SystemFileConcept ?? false;
            }
            var logEntry =  new LogEntry { EntryTypeId = "6.C_14_2" }; 
            logEntry.AddTag("ListOfNames", functionalDescriptionUsageList.SmartToString("¤¤"));
            this.ReportLogEntry(logEntry);

            if (containsDigitalDocuments
                && !(functionalDescriptions.Contains(FunctionalDescription.Dokumenttitel)
                    || functionalDescriptions.Contains(FunctionalDescription.Dokumentdato)))
            {
                logEntry = new LogEntry { EntryTypeId = "6.A_6" };
                this.ReportLogEntry(logEntry);
            }

            if (functionalDescriptions.Contains(FunctionalDescription.Dokumentidentifikation)
                && !containsDigitalDocuments) 
            {
                this.ReportLogEntry(new LogEntry { EntryTypeId = "6.C_14" }); // OK
            }

            if (!functionalDescriptions.Contains(FunctionalDescription.Dokumentidentifikation)
                && containsDigitalDocuments)
            {
                this.ReportLogEntry(new LogEntry { EntryTypeId = "6.C_14_1" });
            }

            if (!systemFileConcept)
            {
                if (functionalDescriptions.Contains(FunctionalDescription.Sagsidentifikation))
                {
                    logEntry = new LogEntry { EntryTypeId = "6.C_13" };
                    logEntry.AddTag("FunctionalDescription", FunctionalDescription.Sagsidentifikation.ToString());
                    this.ReportLogEntry(logEntry);
                }

                if (functionalDescriptions.Contains(FunctionalDescription.Sagstitel))
                {
                    logEntry = new LogEntry { EntryTypeId = "6.C_13" };
                    logEntry.AddTag("FunctionalDescription", FunctionalDescription.Sagstitel.ToString());
                    this.ReportLogEntry(logEntry);
                }
            }

            if (!(functionalDescriptions.Contains(FunctionalDescription.Sagsidentifikation)
                || functionalDescriptions.Contains(FunctionalDescription.Sagstitel))
                && systemFileConcept)
            {
                this.ReportLogEntry(new LogEntry { EntryTypeId = "6.A_7" });
                
            }
        }

        public FunctionalDescriptionTest(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}