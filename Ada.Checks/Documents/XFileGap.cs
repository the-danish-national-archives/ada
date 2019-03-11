namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.FileSystem;

    #endregion

    public class XFileGap : AdaAvViolation
    {
        #region  Constructors

        public XFileGap(string tagType, string path)
            : base(tagType)
        {
            Path = path;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion

        #region

        // TODO make unit tests for this - notice lots of fun edgecases
        protected static IEnumerable<AdaAvCheckNotification> Check
        (IEnumerable<FileSystemFile> enumerateDocFilesOrder,
            Func<string, XFileGap> creator)
        {
//            bool hasAGap = false;
            FileSystemEntry lastFile = null;
            var numbers = new List<int>();
            foreach (var file in enumerateDocFilesOrder)
            {
                int fileNumber;

                if (lastFile?.RelativePath != file.RelativePath)
                {
                    if (HasAGap(numbers))
                        yield return creator(lastFile.RelativePath);

                    numbers.Clear();
                    lastFile = file;
                }

                if (int.TryParse(System.IO.Path.GetFileNameWithoutExtension(file.Name), out fileNumber))
                    numbers.Add(fileNumber);
            }

            // and the last one
            if (HasAGap(numbers))
                yield return creator(lastFile.RelativePath);
        }

        private static bool HasAGap(List<int> numbers)
        {
            numbers.Sort();
            var last = 0;
            foreach (var number in numbers)
            {
                if (number == last)
                    continue;
                if (number != last + 1)
                    return true;
                last = number;
            }

            return false;
        }

        #endregion
    }
}