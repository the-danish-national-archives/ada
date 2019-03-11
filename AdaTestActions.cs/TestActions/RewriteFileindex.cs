namespace Ada.ADA.Common.TestActions
{
    using System.IO;

    using global::Ada.ADA.EntityLoaders;
    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Repositories;

    public class RewriteFileindex : AdaActionBase<string>
    {
        protected override void OnRun(string path)
        {
            if (File.Exists(path + ".original"))
            {
                File.Delete(path + ".original");
            }

            this.RenameFile(path, path + ".original");

            using (var fs = new FileStream(path, FileMode.Create))
            {
                var writer = new FileIndexWriter();
                writer.Open(fs);

                var repo = new AdaFileIndexRepo(
                            new AdaTestUowFactory(
                            this.Mapping.AVID, "test",
                            new DirectoryInfo(Properties.Settings.Default.DBCreationFolder)),
                            1000);
                foreach (var file in repo.EnumerateFiles())
                {
                    writer.WriteElement(file, this.Mapping.AVID);
                }
                repo.Dispose();
                writer.Close();
            }

            File.Delete(path + ".original");
        }

        private void RenameFile(string oldFilename, string newFilename) //Proper rename instead?
        {
            var file = new FileInfo(oldFilename);

            if (file.Exists)
            {
                file.MoveTo(newFilename);
            }
        }

        public RewriteFileindex(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }
    }
}