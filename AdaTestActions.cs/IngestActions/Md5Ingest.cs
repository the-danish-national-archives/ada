namespace Ada.ADA.Common.IngestActions
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    using Ada.Common;
    using Ada.Log;
    using Ada.Repositories;

    using Ra.DomainEntities.FileIndex.Extensions;

    public class Md5Ingest : AdaActionBase<IAdaUowFactory>
    {
        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var repo = new AdaStructureRepo(
                           factory,
                            1000))
            {
                var md5 = MD5.Create();
                foreach (var file in repo.EnumerateFiles())
                {
                    var path = this.Mapping.GetMediaPath(Int32.Parse(file.MediaNumber)) + file.RelativePathAndFile();
                    if (File.Exists(path))
                    {
                        using (var stream = File.OpenRead(path))
                        {
                            var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                            file.Md5 = hash;
                            repo.UpdateMD5(file);
                        }
                    }
                }
                repo.Commit();
            }
        }

        public Md5Ingest(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}