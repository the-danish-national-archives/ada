namespace Ada.ADA.Common.TestActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.FileIndex;
    using Ra.DomainEntities.FileIndex.Extensions;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    public class RewriteXsd : AdaActionBase<IAdaUowFactory>
    {

        protected override void OnRun(IAdaUowFactory factory)
        {
            var entries = new List<LogEntry>();
            using (var testuow = (UnitOfWork)factory.GetUnitOfWork())
            {
                testuow.BeginTransaction();
                foreach (var table in testuow.GetRepository<Table>().All().AsParallel())
                {
                    var fileRepo = testuow.GetRepository<FileIndexEntry>();
                    var xsdMetaData = fileRepo.All().FirstOrDefault(x => x.Extension == ".xsd" && x.FileName == table.Folder);

                    if (xsdMetaData == null)
                        continue;

                    var md5 = MD5.Create();
                    using (var memStream = table.GetXmlSchemaStream())
                    {
                        var hash = BitConverter.ToString(md5.ComputeHash(memStream)).Replace("-", string.Empty);

                        //if (hash != xsdMetaData.Md5) //TODO: only rewrite if existing xsd doesn't match generated
                        //{
                        var path = this.Mapping.GetMediaPath(Int32.Parse(xsdMetaData.MediaNumber))
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

            foreach (var logEntry in entries)
            {
                this.ReportLogEntry(logEntry);
            }
        }

        public RewriteXsd(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}