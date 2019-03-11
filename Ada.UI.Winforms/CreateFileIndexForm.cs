namespace Ada.UI.Winforms
{
    #region Namespace Using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Linq;
    using Ra.Common;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Xml;

    #endregion

    public partial class CreateFileIndexForm : Form
    {
        #region  Fields

        private bool stop;

        #endregion

        #region  Constructors

        public CreateFileIndexForm()
        {
            InitializeComponent();
        }

        #endregion

        #region

        private void AfbrydButton_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        /// <summary>
        ///     Calculates the hash.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private static string CalculateHash(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var md5 = MD5.Create();
                var checsum = md5.ComputeHash(stream);
                var hash = BitConverter.ToString(checsum).Replace("-", string.Empty).ToUpper();
                stream.Close();
                return hash;
            }
        }

        private static TimeSpan CalculateTimeSpend(DateTime startTime, int i, ArrayList al, out int h, out int m)
        {
            var span = DateTime.Now.Subtract(startTime);
            double secUsed = span.Seconds + span.Minutes * 60 + span.Hours * 60 * 60 + span.Days * 24 * 60 * 60;
            var secLeft = secUsed / i * (al.Count - i);
            var hoursLeft = secLeft / (60 * 60);
            h = (int) Math.Floor(hoursLeft);
            m = (int) ((secLeft - h * 60 * 60) / 60);
            return span;
        }

        /// <summary>
        ///     Closes the XML file Stream.
        /// </summary>
        /// <param name="xw">The xw.</param>
        /// <param name="xmlFileStream">The XML file Stream.</param>
        private static void CloseXmlFileStream(XmlTextWriter xw, FileStream xmlFileStream)
        {
//Slut element skrives
            xw.WriteEndElement();
            xw.WriteEndDocument();
            xw.Flush();
            xw.Close();
            xmlFileStream.Close();
        }


        /// <summary>
        ///     Skab fileIndex.xml.
        ///     Stien til den nye fileIndex.xml er identisk med den eksisterende (AV'ens fileIndex.xml overskrives)
        /// </summary>
        /// <param name="AV"></param>
        /// <param name="drives"></param>
        /// <returns></returns>
        public bool CreateFileIndex(string AV, List<string> drives, bool noDocs)
        {
            var fileIndexOut = "";


            //Find sti til fileIndex.xml
            foreach (var drive in drives)
                if (Directory.Exists(drive[0] + @":\" + AV + @".1\Indices"))
                {
                    fileIndexOut = drive[0] + @":\" + AV + @".1\Indices\fileIndex.xml";
                    break;
                }

            if (fileIndexOut == "") //Indices mappe ikke fundet
                return false;

            //*** Selve skabelsen af fileIndex ... ****
            return CreateFileIndex(AV, drives, fileIndexOut, noDocs ? fileIndexOut : null);
        }


        /// <summary>
        ///     Skab fileIndex.xml.
        ///     Stien til den nye fileIndex.xml kan være en anden end stien til AV'ens eksisterende fileIndex.xml
        /// </summary>
        /// <param name="AV"></param>
        /// <param name="drives"></param>
        /// <param name="fileIndexOut"></param>
        /// <param name="fileIndexIn"></param>
        /// <returns></returns>
        public bool CreateFileIndex(string AV, List<string> drives, string fileIndexOut, string fileIndexIn = null)
        {
            FileInfo fileIndexInFileInfo = null;
            if (fileIndexIn != null)
            {
                fileIndexInFileInfo = new FileInfo(fileIndexIn);
                if (!fileIndexInFileInfo.Exists)
                    return false;
            }


            //For hvert drev
            var al = new ArrayList();
            foreach (var drive in drives)
            {
                //her skal ledes efter alle de mapper som indeholder første del af AV navn ....
                //string[] AVfolders = Directory.GetDirectories(drive + @":\", @AV + ".*");
                label1.Text = "Skaber liste over filer ... ";
                var AVfolders = Directory.GetDirectories(drive, AV + ".*");
                foreach (var AVfolder in AVfolders)
                {
                    label2.Text = "Mappe: " + AVfolder;
                    Refresh();
                    Application.DoEvents();

                    var fileNames = Directory.GetFiles(AVfolder, "*.*", SearchOption.AllDirectories);
                    if (fileIndexInFileInfo == null)
                        al.AddRange(fileNames);
                    else
                        al.AddRange(fileNames.Where(s => !IsInDocFolder(s.Remove(0, AVfolder.Length))).ToArray());
                }
            }

            if (al.Count == 0) return false;

            al.Sort();

            XmlTextWriter xw;
            var xmlFileStream = CreateNewXmlFile(fileIndexOut, out xw);


            var startTime = DateTime.Now;

            //For hver fil ...
            try
            {
                for (var i = 0; i < al.Count; i++)
                {
                    if (stop)
                    {
                        xw.Close();
                        xmlFileStream.Close();
                        return false;
                    }

                    var currentFileInfo = new FileInfo(al[i].ToString());
                    const string fileIndexName = "fileIndex.xml";
                    if (currentFileInfo.Name != fileIndexName)
                    {
                        var hash = CalculateHash(currentFileInfo.FullName);
                        WriteValuesToIndexfile(xw, currentFileInfo.DirectoryName.Substring(3), currentFileInfo.Name,
                            hash);
                    }


                    //For hver 10 filer
                    if (i % 10 == 0)
                    {
                        int h, m;
                        var span = CalculateTimeSpend(startTime, i, al, out h, out m);
                        WriteProgress(currentFileInfo, h, m, span);
                        Application.DoEvents();
                    }
                }


                if (fileIndexInFileInfo != null)
                {
                    // Find input fileindex
                    var archivalXmlReader = new ArchivalXmlReader(new XmlEventFilter());
                    using (var stream = new BufferedProgressStream(fileIndexInFileInfo))
                    {
                        archivalXmlReader.Open(stream, null); //, targetXmlCouplet.SchemaStream

                        foreach (var element in archivalXmlReader.ElementStream("f"))
                        {
                            XNamespace ns = "http://www.sa.dk/xmlns/diark/1.0";

                            var folderName = element.Element(ns + "foN")?.Value;
                            var root = folderName.GetRootFolder();

                            var fileName = element.Element(ns + "fiN")?.Value;

                            if (!IsInDocFolder(folderName?.Remove(0, root.Length)))
                                continue;

                            WriteValuesToIndexfile(xw, folderName, fileName,
                                element.Element(ns + "md5")?.Value);
                        }

                        archivalXmlReader.Close();
                    }
                }
            }
            catch (Exception)
            {
                xw.Close();
                xmlFileStream.Close();
                File.Delete(fileIndexOut + ".tmp");
                return false;
            }

            CloseXmlFileStream(xw, xmlFileStream);

            SaveNewFileIndex(fileIndexOut);

            return true;
        }

        private static FileStream CreateNewXmlFile(string fileIndexOut, out XmlTextWriter xw)
        {
            var xmlFileStream = new FileStream(fileIndexOut + ".tmp", FileMode.Create);
            //opretter ny fileIndex.xml fil med en xml deklaration             
            xw = new XmlTextWriter(xmlFileStream, new UTF8Encoding());
            xw.Formatting = Formatting.Indented;
            xw.WriteStartDocument();
            xw.WriteStartElement("fileIndex"); //root element
            xw.WriteAttributeString("xsi:schemaLocation",
                "http://www.sa.dk/xmlns/diark/1.0 ../Schemas/standard/fileIndex.xsd");
            xw.WriteAttributeString("xmlns", "http://www.sa.dk/xmlns/diark/1.0");
            xw.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            return xmlFileStream;
        }

        private static bool IsInDocFolder(string rela)
        {
            return rela.StartsWith("\\Documents\\")
                // do not skip ContextDocumentation, see ADA-38
//                 || rela.StartsWith("\\ContextDocumentation\\")
                ;
        }

        private static void SaveNewFileIndex(string fileIndexOut)
        {
            File.Delete(fileIndexOut);
            File.Move(fileIndexOut + ".tmp", fileIndexOut);
        }

        private void WriteProgress(FileInfo currentFileInfo, int h, int m, TimeSpan span)
        {
            label1.Text = string.Format("MD5 af {0}", currentFileInfo.FullName);
            label2.Text =
                string.Format("Estimeret slut om: {0} timer og {1} min." + " (varighed {2}:{3}:00)",
                    h.ToString(), m.ToString().PadLeft(2, '0'),
                    (span.Days * 24 + span.Hours).ToString(),
                    span.Minutes.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        ///     Writes the values to indexfile.
        /// </summary>
        /// <param name="xw">The indexfile as a Stream</param>
        /// <param name="folder">The folder.</param>
        /// <param name="shortFilename">The short filename.</param>
        /// <param name="hash">The hash.</param>
        private static void WriteValuesToIndexfile(XmlTextWriter xw, string folder, string shortFilename, string hash)
        {
            xw.WriteStartElement("f");
            xw.WriteElementString("foN", folder);
            xw.WriteElementString("fiN", shortFilename);
            xw.WriteElementString("md5", hash);
            xw.WriteEndElement();
            xw.Flush();
        }

        #endregion
    }
}