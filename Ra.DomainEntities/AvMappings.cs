namespace Ada.Common
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Ra.DomainEntities;

    #endregion

    [Serializable]
    public class AVMapping
    {
        #region  Fields

        private readonly List<KeyValuePair<int, string>> _errors = new List<KeyValuePair<int, string>>();

        private readonly Dictionary<int, string> _media = new Dictionary<int, string>();


        /// <summary>
        ///     Pretend this is Active root directories instead
        /// </summary>
        public List<string> RootDirectories;

        #endregion

        #region  Constructors

        public AVMapping(AViD AVID, List<string> directories)
        {
            this.AVID = AVID;
            load(directories);
            RootDirectories = directories;
        }

        #endregion

        #region Properties

        public AViD AVID { get; }

        public List<string> DistinctMappings => new List<string>(_media.Values.Distinct());

        #endregion

        #region

        public static AVMapping CreateMapping(AViD AVID, List<string> whiteList = null)
        {
            var directories = GetMediaDirectories(AVID, whiteList);

            return directories.Count < 1 ? null : new AVMapping(AVID, directories);
        }


        private static List<string> GetDriveList()
        {
            var res = new List<string>();
            foreach (var drive in DriveInfo.GetDrives())
                if (drive.IsReady)
                    res.Add(drive.ToString());

            return res;
        }

        public string GetLocalPath(int mediaNumber)
        {
            string localPath;
            _media.TryGetValue(mediaNumber, out localPath);
            return localPath;
        }

        public static List<string> GetMediaDirectories(AViD AVID, IEnumerable<string> rootDirectories)
        {
            rootDirectories = rootDirectories ?? GetDriveList();

            var dirs = rootDirectories.Select(d => new {s = d, d = new DriveInfo(d)})
                .Where(c => c.d.IsReady)
                .Select(c => new DirectoryInfo(c.s))
                .SelectMany(d => d.GetDirectories(AVID.FullID + ".*"))
                .Select(d => d.FullName)
                .ToList();
            int outInt;
            var filteredDirs = dirs.Where(d => d.Split(new[] {"AVID"}, StringSplitOptions.None)[1].Split('.').Length == 4 && int.TryParse(d.Split(new[] {"AVID"}, StringSplitOptions.None)[1].Split('.').ToArray()[3], out outInt)).ToList();
            return filteredDirs;
        }


        public string GetMediaPath(int mediaNumber)
        {
            var localPath = GetLocalPath(mediaNumber);
            return Path.Combine(localPath, AVID.FullID + "." + mediaNumber);
        }

        /// <summary>
        ///     Old method to be removed, use GetLocalPath
        /// </summary>
        /// <param name="mediaNumber"></param>
        /// <returns></returns>
        public string GetMediaRoot(int mediaNumber)
        {
            return GetLocalPath(mediaNumber);
        }

        public string GetRelativePath(FileSystemInfo fileInfo)
        {
            var collector = new StringBuilder();
            foreach (var bah in fileInfo.FullName.Split(Path.DirectorySeparatorChar))
            {
                if (AViD.ExtractMediaNumber(bah) != null) collector.Clear();
                collector.Append(Path.DirectorySeparatorChar);
                collector.Append(bah);
            }

            return collector.ToString();
        }

        /// <summary>
        ///     true hvis der ikke findes dubletter og ugyldige medianumbers
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return _errors.Count == 0;
        }


        private void load(List<string> directories)
        {
            _media.Clear();
            _errors.Clear();

            foreach (var dirName in directories)
            {
                int mediaNumber;
                if (!int.TryParse(AViD.ExtractMediaNumber(dirName), out mediaNumber))
                {
                    _errors.Add(new KeyValuePair<int, string>(2, dirName)); //ikke integer
                    break;
                }

                if (_media.ContainsKey(mediaNumber))
                {
                    _errors.Add(new KeyValuePair<int, string>(1, dirName)); //dublet
                    break;
                }

                var mediaName = Directory.GetParent(dirName).FullName;

                if (mediaName.Last() != Path.DirectorySeparatorChar)
                    mediaName += Path.DirectorySeparatorChar;

                _media.Add(mediaNumber, mediaName);
            }
        }

        #endregion
    }
}