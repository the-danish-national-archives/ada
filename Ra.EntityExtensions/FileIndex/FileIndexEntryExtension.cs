namespace Ra.EntityExtensions.FileIndex
{
    #region Namespace Using

    using System.IO;
    using Ada.Common;
    using Common;
    using DomainEntities.FileIndex;

    #endregion

    public static class FileIndexEntryExtensions
    {
        #region

        public static BufferedProgressStream GetAsStream(this FileIndexEntry fileIndexEntry, AVMapping avMapping)
        {
            var pathToFile = Path.Combine(
                avMapping.GetMediaPath(int.Parse(fileIndexEntry.MediaNumber)),
                fileIndexEntry.RelativePathAndFile().Trim(Path.DirectorySeparatorChar));
            if (!new FileInfo(pathToFile).Exists)
                return null;
            return new BufferedProgressStream(new FileInfo(pathToFile));
        }

        public static string RelativePathAndFile(this FileIndexEntry file)
        {
            return Path.Combine(file.RelativePath, string.Concat(file.FileName, file.Extension));
        }

        #endregion
    }
}