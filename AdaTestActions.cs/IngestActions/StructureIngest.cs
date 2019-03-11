namespace AdaTestActions.IngestActions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using global::Ada.ActionBase;
    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Repositories;

    using Ra.DomainEntities;

    public class StructureIngest : AdaIngestAction<List<string>>
    {
        public StructureIngest(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, AdaStructureRepo structureRepo)
            : base(processLog, testLog, mapping, structureRepo)
        {
        }


        protected override void OnRun(List<string> dirs)
        {
                var hasDocuments = false;
                var hasTables = false;
              
                foreach (string dir in dirs)
                {
                    var mediaFolder = new DirectoryInfo(dir);
                    var mediaNo = AViD.ExtractMediaNumber(mediaFolder.Name);
                    var rootFolder = mediaFolder.Parent;

                    foreach (var fi in mediaFolder.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).AsParallel()) 
                    {
                        if (fi is FileInfo)
                        {
                            structureRepo.AddFile(mediaNo, mediaFolder, fi as FileInfo);
                        }

                        if (fi is DirectoryInfo)
                        {
                            if (fi.Name == "Tables")
                            {
                                hasTables = true;
                            }

                            if (fi.Name == "Documents")
                            {
                                hasDocuments = true;
                            }

                            structureRepo.AddFolder(mediaNo, rootFolder, fi as DirectoryInfo);
                        }
                    }

                    structureRepo.AddMedia(mediaNo, rootFolder, this.Mapping, hasDocuments, hasTables);
                }

                structureRepo.Commit();
        }
    }
}


