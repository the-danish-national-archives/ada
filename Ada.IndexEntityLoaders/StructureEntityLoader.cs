namespace IndexEntityLoaders
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ada.Common;
    using Ra.DomainEntities.FileSystem;

    #endregion

    public class StructureEntityLoader
    {
        #region  Fields

        private readonly AVMapping mapping;

        #endregion

        #region  Constructors

        public StructureEntityLoader(AVMapping mapping)
        {
            this.mapping = mapping;
        }

        #endregion

        #region

        public IEnumerable<FileSystemFolder> EnumerateFileSystemFolders()
        {
            foreach (var g in mapping.RootDirectories.Select(s => new DirectoryInfo(s).Name).GroupBy(n => n).Where(g => g.Count() > 1)) throw new DuplicateMediaNumbersException(g.Key, g.Count());

            foreach (var dir in mapping.RootDirectories)
            {
                var mediaFolder = new DirectoryInfo(dir);
                var entry = new FileSystemFolder
                {
                    Level = 0,
                    Name = mediaFolder.Name,
                    MediaNumber = Convert.ToInt32(mediaFolder.FullName.Split('.').Last())
                };
                yield return entry;

                foreach (var folder in EnumerateFolders(mediaFolder, entry))
                    yield return folder;
            }
        }

//        private string GetRelativePath(FileSystemEntry entry, string fullPath)
//        {          
//            return Path.GetDirectoryName(fullPath)?
////                .Replace(Path.GetPathRoot(fullPath), string.Empty)
//.Replace(mapping.GetRelativePath())
//                .Trim(Path.DirectorySeparatorChar);
//
//        }


        private IEnumerable<FileSystemFolder> EnumerateFolders(DirectoryInfo dirInfo, FileSystemFolder ancestor)
        {
            var fileCount = 0;
            var descendants = new List<Tuple<DirectoryInfo, FileSystemFolder>>();

            var relativePath = mapping.GetRelativePath(dirInfo).Trim(Path.DirectorySeparatorChar);


            foreach (var fi in dirInfo.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly).AsParallel())
            {
                var descendant = new FileSystemEntry();
                if (fi is FileInfo)
                {
                    descendant = new FileSystemFile {Size = (fi as FileInfo).Length, Extension = fi.Extension};
                    fileCount++;
                }

                if (fi is DirectoryInfo)
                {
                    descendant = new FileSystemFolder();
                    descendants.Add(new Tuple<DirectoryInfo, FileSystemFolder>(fi as DirectoryInfo, (FileSystemFolder) descendant));
                }

                (descendant.Ancestors as List<FileSystemEntry>)?.AddRange(ancestor.Ancestors);

                descendant.Ancestors.Add(ancestor);
                descendant.Level = ancestor.Level + 1;
                descendant.RelativePath = relativePath;
                //this.GetRelativePath(descendant, fi.FullName);
                descendant.Name = fi.Name;
                descendant.TimeStamp = fi.LastWriteTime;
                descendant.MediaNumber = ancestor.MediaNumber;

                ancestor.Contents.Add(descendant);
            }

            ancestor.FileCount = fileCount;
            ancestor.FolderCount = descendants.Count;

            yield return ancestor;

            foreach (var subFolder in descendants)
            foreach (var dir in EnumerateFolders(subFolder.Item1, subFolder.Item2))
                yield return dir;
        }

        #endregion
    }

    public class DuplicateMediaNumbersException : Exception
    {
        #region  Constructors

        public DuplicateMediaNumbersException(string name, int count)
        {
            Name = name;
            Count = count;
        }

        #endregion

        #region Properties

        public int Count { get; set; }
        public string Name { get; set; }

        #endregion
    }
}