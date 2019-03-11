// ReSharper disable RedundantAssignment

namespace Ada.Test.AutoRunTestsuite
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Main;
    using NUnit.Framework;
    using Ra.Common.ExtensionMethods;
    using SevenZip;

    #endregion

    [TestFixture]
    public class TestSuiteTesting
    {
        private class AfleveringsExtractor
        {
            #region  Constructors

            static AfleveringsExtractor()
            {
                ConfigurationManager.AppSettings["7zLocation"] = "7z.dll"; // Path.Combine(Path.GetDirectoryName(typeof(AfleveringsExtractor).Assembly.Location), "7z.dll");
            }

            #endregion

            #region Properties

            public static IEnumerable<TestCaseData> TestCases
            {
                get
                {
                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                    //                    http://git-intern.sa.dk/sakjf/ADA_Testsuite.git
                    var rootDirectory = new DirectoryInfo(@"..\..\..\..\ADA_Testsuite");
                    //                    var rootDirectory = new DirectoryInfo(@"E:\prog\ADA\ada_temp");

                    var rootAndDepthOne = new[] {rootDirectory}.Union(rootDirectory.EnumerateDirectories());
                    var testCasesFound = false;

                    foreach (var file in rootAndDepthOne.SelectMany(d => d.EnumerateFiles("AVID.*.zip")))
                    {
                        if (!IsLowestMediaNumber(file))
                            continue;

                        var path = file.FullName;
                        var name = file.FullName.Replace(rootDirectory.FullName, string.Empty).Trim(Path.DirectorySeparatorChar);
                        var category = file.Directory?.Name ?? string.Empty;

                        var otherDirectories = FindOtherRoots(file);
                        var otherRoots =
                            otherDirectories.Select(d => d.FullName).SmartToString(Path.PathSeparator.ToString());

                        yield return
                            new TestCaseData(rootDirectory.FullName, name, path, otherRoots, category).SetName(name)
                                .SetCategory(category);
                        testCasesFound = true;
                    }

                    if (!testCasesFound)
                        Assert.Fail($"No zip files for testing found. Looked in (including subfolders at one depth) {rootDirectory.FullName}");
                }
            }

            #endregion

            #region

            /// <summary>
            ///     Returns any sub directories that contains other root directories. At the moment that is the folder "Z" if
            ///     it exists in the parent folder, and it have any subfolders with the same AViD (potentially another medie numer)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            private static IEnumerable<DirectoryInfo> FindOtherRoots(FileInfo file)
            {
                if (file.Directory == null) yield break;

                var name = file.Name;
                var AVidName = GetAvidName(name);

                var candidate = new DirectoryInfo(Path.Combine(file.Directory.FullName, "Z"));

                if (!candidate.Exists) yield break;

                var subDirectories = candidate.EnumerateFiles(AVidName + "*.zip");

                if (subDirectories.Any())
                    yield return candidate;
            }

            private static bool IsLowestMediaNumber(FileInfo file)
            {
                if (file.Directory == null) return true;

                var name = file.Name;
                var AVidName = GetAvidName(name);
                var mediaNumber = name.Replace(AVidName, string.Empty);

                foreach (var candidate in file.Directory.EnumerateFiles(AVidName + "*.zip"))
                {
                    var mediaNumberC = candidate.Name.Replace(AVidName, string.Empty);
                    if (string.CompareOrdinal(mediaNumber, mediaNumberC) > 0) return false;
                }

                return true;
            }

            #endregion
        }

        private static string GetAvidName(string name)
        {
            var regex = new Regex("(?<n>AVID[.]SA[.]\\d*)");
            return regex.Match(name).Groups["n"].Value;
        }

        //        private string UnZip(string otherPath, string avidName)
        //        {
        //            UnZip(Path.Combine(otherPath, $"{avidName}.1.zip"));
        //
        //            return otherPath;
        //        }

        private void UnZip(string pathZips, string avidName)
        {
            //            var pathLength = pathZip.Length - 4;
            //            if (pathLength < 0) return "";

            //            var res = pathZip.Substring(0, pathLength);

            //            var fileInfo = new FileInfo(pathZips);
            //            var avidName = GetAvidName(fileInfo.Name);

            var topDirectoryInfo = new DirectoryInfo(pathZips);

            foreach (var zipFile in topDirectoryInfo.EnumerateFiles($"{avidName}*.zip"))
            {
                var zipFileLength = zipFile.FullName.Length - 4;
                var zipFileRes = zipFile.FullName.Substring(0, zipFileLength);

                Assert.IsTrue(zipFileRes.Contains("AVID.SA"), $"I'm too scared to delete '{zipFileRes}'");

                var zipDirectoryInfo = new DirectoryInfo(zipFileRes);

                EnsureDeleted(zipDirectoryInfo);

                using (var extractor = new SevenZipExtractor(zipFile.FullName))
                {
                    extractor.ExtractArchive(zipDirectoryInfo.FullName);
                }

                //                
                //                using (var zip = new ZlibStream(z) ZipFile.Read(zipFile.FullName))
                //                {
                //                    zip.ExtractAll(zipDirectoryInfo.FullName);
                //                }

                zipDirectoryInfo.Refresh();

                EnsureAtTopDirectory(zipDirectoryInfo);
            }
        }

        private static void EnsureAtTopDirectory(DirectoryInfo zipDirectoryInfo)
        {
            var zipFileRes = zipDirectoryInfo.FullName;
            var topDirectoryInfo = zipDirectoryInfo.Parent;

            var misplaced = zipDirectoryInfo.GetDirectories(zipDirectoryInfo.Name, SearchOption.TopDirectoryOnly);
            if (misplaced.Any())
            {
                var tempName = Path.GetRandomFileName();
                var tempDirectoryInfo = new DirectoryInfo(Path.Combine(topDirectoryInfo?.FullName ?? "", tempName));
                zipDirectoryInfo.MoveTo(tempDirectoryInfo.FullName);
                tempDirectoryInfo.Refresh();
                misplaced = tempDirectoryInfo.GetDirectories(misplaced[0].Parent?.Name ?? "", SearchOption.TopDirectoryOnly);
                Assert.IsTrue(misplaced.Any());
                misplaced[0].MoveTo(zipFileRes);

                if (!tempDirectoryInfo.EnumerateFiles().Any())
                    tempDirectoryInfo.Delete();
            }
        }

        private static void EnsureDeleted(DirectoryInfo zipDirectoryInfo)
        {
            // loop to counter "Directory not empty" exception from DirectoryInfo helper
            for (var i = 3; i >= 0; i--)
                try
                {
                    zipDirectoryInfo.Refresh();

                    if (zipDirectoryInfo.Exists) zipDirectoryInfo.Delete(true);


                    // Wait for filesystem to update
                    var breakOut = 3;
                    zipDirectoryInfo.Refresh();
                    while (zipDirectoryInfo.Exists && breakOut-- > 0)
                    {
                        Thread.Sleep(100);
                        zipDirectoryInfo.Refresh();
                    }

                    break;
                }
                catch (IOException)
                {
                    //                    throw;
                }
        }

        private static void RunAda(string testName, DirectoryInfo testDir, string otherRoots, DirectoryInfo outputDir)
        {
            using (var proc = SetupAdaProcess(testName, testDir, otherRoots, outputDir))
            {
                var procStarted = false;
                try
                {
                    // To debug child process use: "Microsoft Child Process Debugging Power Tool" extension to visual studio
                    // Remeber to "Enable native code debugging" under Debug in properties of the testproject
                    procStarted = proc.Start();

                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();


                    var waitTime = 3; // 20 minutes -- 17300 took longer than 5 min on testserver
                    if (
                        testName.Contains("17300") // lots of large pictures
                        ||
                        testName.Contains("17175") // a 2gb picture
//                        ||
//                        testName.Contains("17211")
//                        ||
//                        testName.Contains("17136")
                    )
                        waitTime = 120;
#if DEBUG
                    waitTime = 200;
#endif
                    if (!proc.WaitForExit(waitTime * 60 * 1000))
                    {
                        proc.Kill();
                        Assert.Fail($@"Ada did not exit within allocated time ({waitTime} min).");
                    }

                    proc.CancelOutputRead();
                    proc.CancelErrorRead();

                    proc.WaitForExit();

                    Assert.AreEqual(Program.SuccessCode, proc.ExitCode, "ADA did not progress exit correctly.");
                }
                finally
                {
                    if (procStarted && !proc.HasExited) proc.Kill();
                }
            }
        }

        private static Process
            SetupAdaProcess(string testName, DirectoryInfo testDir, string otherRoots, DirectoryInfo outputDir)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            outputDir.Parent?.Create();

            if (outputDir.Exists)
                outputDir.Delete(true);

            var settingsFile = $@"{testDir.Parent?.FullName}\settings.{testDir.Name}.xml";
            if (!new FileInfo(settingsFile).Exists)
                settingsFile = $@"{testDir.Parent?.FullName}\settings.xml";


            var elapsedTime = new Stopwatch();

            elapsedTime.Start();
            var proc = new Process();
            var outputText = new StringBuilder();
            Func<string, Action<string>> datareader = preText => text =>
            {
                var combinedText = $"{elapsedTime.Elapsed.TotalSeconds:N3}: {preText}: {text}";
                Debug.WriteLine(combinedText);
                Console.WriteLine(combinedText);
                outputText.AppendLine(combinedText);
            };


            var reportOutput = datareader("ADA output");
            proc.OutputDataReceived += (sender, args) => reportOutput(args.Data);

            proc.StartInfo.RedirectStandardOutput = true;

            var reportError = datareader("ADA ERROR -- bat oerror");
            proc.ErrorDataReceived += (sender, args) => reportError(args.Data);

            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.FileName = "Ada.exe";


            var inputPath = testDir.FullName;
            // tests in 4.B.1 have the test in a sub directory
            var subDirectory = testDir.EnumerateDirectories(testDir.Name).FirstOrDefault();
            if (subDirectory != null) inputPath = subDirectory.FullName;

            var arguments = string.Join(
                " ", $@"-i {inputPath.AsQuoted()}", $@"-s {settingsFile.AsQuoted()}", !string.IsNullOrWhiteSpace(otherRoots) ? $@"-r {otherRoots.AsQuoted()}" : "", @"-V", $@"-f {outputDir.FullName.AsQuoted()}", $@"-o {outputDir.FullName.AsQuoted()}");
            proc.StartInfo.Arguments = arguments;

            datareader("AutoRun")($"argumnets: {arguments}");

            return proc;
        }

        [Test]
        [TestCaseSource(typeof(AfleveringsExtractor), nameof(AfleveringsExtractor.TestCases))]
        public void TestTestsFromTestsuite(string mainFolder, string testName, string pathZip, string otherRootsZip, string category)
        {
            var fileInfo = new FileInfo(pathZip);
            //            var avidName = GetAvidName(fileInfo.Name);

            var path = pathZip.Remove(pathZip.Length - 4);

            var avidName = GetAvidName(testName);

            UnZip(fileInfo.Directory?.FullName, avidName);
            foreach (var otherRoot in otherRootsZip.Split(Path.PathSeparator))
            {
                if (string.IsNullOrWhiteSpace(otherRoot)) continue;
                UnZip(otherRoot, avidName);
            }

            var otherRoots = otherRootsZip;
            //                otherRootsZip.Length > 0
            //                                 ? otherRootsZip
            //                                 : otherRootsZip.Split(Path.PathSeparator)
            //                                       .Select(o => )
            //                                       .SmartToString(Path.PathSeparator.ToString());


            var testDir = new DirectoryInfo(path);
            Assert.IsNotNull(testDir, $"Path invalid for testing '{pathZip}'");
            Assert.IsTrue(testDir.Exists, $"Path for testing does not exist '{pathZip}'");

            //            path.Replace(mainFolder, "").Replace(Path.DirectorySeparatorChar, '.')
            var expectedLogEntriesPath = Path.Combine(mainFolder, "Results", testDir.Name + "." + category + ".LogEvents.xml");
            var eventsExpected = LogEntrySimple.ReadEntryLogFile(expectedLogEntriesPath);

            var logPath = Path.Combine(@".\test", testName);
            var outputPath = Path.Combine(logPath, @"output");


            var outputDir = new DirectoryInfo(outputPath);

            RunAda(testName, testDir, otherRoots, outputDir);


            var eventsActual = LogEntrySimple.GetEventLogsFromDb(testDir, outputDir);


            var badXml = new FileInfo(expectedLogEntriesPath + ".bad");
            if (badXml.Exists)
                badXml.Delete();


            var withBadFormattedText = eventsActual.FirstOrDefault(l => l.FormattedText.Contains('{') || l.FormattedText.Contains('}'));
            if (withBadFormattedText != null)
            {
                LogEntrySimple.WriteEventLogFile(badXml.FullName, eventsActual);
                Assert.Inconclusive($"Bad errorText found for {withBadFormattedText},but actual values stored in {badXml.FullName}.");
            }


            if (eventsExpected == null)
            {
                LogEntrySimple.WriteEventLogFile(badXml.FullName, eventsActual);
                Assert.Inconclusive($"Unable to get expected results (looked in {expectedLogEntriesPath}), but actual values stored in {badXml.FullName}, rename the bad one if it is correct.");
            }

            var enumeratorExpected = eventsExpected.GetEnumerator();
            LogEntrySimple lastExpected = null;
            var enumeratorActual = eventsActual.GetEnumerator();
            LogEntrySimple lastActual = null;


            var isEqual = true;

            while (true)
            {
                var moveNextExpected = enumeratorExpected.MoveNext();
                var moveNextActual = enumeratorActual.MoveNext();
                if (!moveNextExpected)
                {
                    if (moveNextActual) isEqual = false;

                    break;
                }

                if (!Equals(enumeratorExpected.Current, enumeratorActual.Current))
                {
                    isEqual = false;
                    if (enumeratorActual.Current != null)
                    {
                        enumeratorActual.Current.ChangeNoticed = true;
                    }
                    else
                    {
                        if (lastActual != null)
                            lastActual.ChangeNoticed = true;
                    }

                    break;
                }

                lastExpected = enumeratorExpected.Current;
                lastActual = enumeratorActual.Current;
            }

            if (!isEqual)
            {
                LogEntrySimple.WriteEventLogFile(badXml.FullName, eventsActual);
                Assert.Fail($@"LogEntries are not the same, compare {expectedLogEntriesPath} and {badXml.FullName}
, replace the expected if the newer is better.
Difference is around the elements (expected):'
{lastExpected?.ToString() ?? "<none>"}
---
{enumeratorExpected.Current?.ToString() ?? "<none>"}
---
{(enumeratorExpected.MoveNext() ? enumeratorExpected.Current?.ToString() ?? "<none>" : "<none>")}
' != (actual):'
{lastActual?.ToString() ?? "<none>"}
---
{enumeratorActual.Current?.ToString() ?? "<none>"}
---
{(enumeratorActual.MoveNext() ? enumeratorActual.Current?.ToString() ?? "<none>" : "<none>")}
' ");
            }
        }
    }
}