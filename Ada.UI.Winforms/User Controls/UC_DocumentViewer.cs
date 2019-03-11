namespace Ada.UI.Winforms.User_Controls
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Common;
    using Printing;
    using Ra.DocumentInvestigator.OldForWinforms;

    #endregion

    public partial class UC_DocumentViewer : UserControl
    {
        #region  Fields

        ///// <summary>
        ///// Struct som indeholder informationer om side i et dokument.
        ///// </summary>
        //public class docPage
        //{
        //    public string fileName;
        //    public int pageNo;
        //}

        /// <summary>
        ///     Liste med dokument sider (en eller flere filer som hver især kan have en eller flere sider)
        /// </summary>
        private readonly List<DocPage> docPageList = new List<DocPage>();

        private readonly UC_DocumentViewerForm ImgViewForm1 = new UC_DocumentViewerForm();
        private readonly UC_DocumentViewerTagForm TagViewForm1 = new UC_DocumentViewerTagForm();

        //Autovisning af dokument
        private readonly Timer ti = new Timer();
        private bool timerForceStop;

        #endregion

        #region  Constructors

        //Initialisering
        public UC_DocumentViewer()
        {
            InitializeComponent();

            //Autovisning af dokumentsider
            ti.Stop();
            ti.Tick += OnTick; //Eventhandler
        }

        #endregion

        #region

        private void AutoVisCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoVisCheckBox.CheckState == CheckState.Checked)
            {
                AutoVisCheckBox.Text = "Auto Stop";
                ti.Interval = 5100 - progressBar1.Value * 100; // 1-50 ~ 100-5000 Default 4000
                timerForceStop = false;
                ti.Start();
            }
            else
            {
                AutoVisCheckBox.Text = "Auto Visning";
                timerForceStop = true;
                ti.Stop();
            }
        }


        private void buttonPrint_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                var docPrint = new SA_DocumentPrint();
                docPrint.PrintDocument(docPageList);
            }
        }


        private void buttonShowTifTags_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (TagViewForm1.Visible == false) TagViewForm1.Show();
                TagViewForm1.ReadTags(
                    docPageList[listBox1.SelectedIndex].FileName,
                    docPageList[listBox1.SelectedIndex].PageNo);
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
            if (splitContainer1.Panel2Collapsed) SkjulStifinderCheckBox.Text = "Vis stifinder";
            if (splitContainer1.Panel2Collapsed == false) SkjulStifinderCheckBox.Text = "Skjul stifinder";
        }


        /// <summary>
        ///     Parameterværdien pattern skal være en kendt dokumenttype fx "*.tif".
        ///     Wildcard *.* og serachPattern fx p* kan benyttes hvis mappe og undermapper udelukkende indeholder dokumenter.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public List<string> GetDocumentsFromFolder(string path, string pattern)
        {
            var docList = new List<string>();

            //***Gennemløb af folders***
            try
            {
                foreach (var folder in Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories))
                foreach (var fil in Directory.GetFiles(folder, pattern))
                    docList.Add(fil);
            }
            catch
            {
                //System Volume Folder m.fl.
                MessageBox.Show("Fejl i forb. med opbygning af mappeliste");
            }

            return docList;
        }


        /// <summary>
        ///     Returnerer Extension som LowerCase uden adskilletegn fx "tif"
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetFileExtension(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            if (ext[0].ToString() == ".")
                return ext.Remove(0, 1); //Fjern dot
            return ext;
            //Fjern dot
        }

        /// <summary>
        ///     Returnerer antal sider for et dokument
        /// </summary>
        /// <param name="fil"></param>
        /// <returns></returns>
        private int GetPageCount(string fil)
        {
            return new TiffLeadToolsProcessor().GetPageCount(fil);
        }

        private void listBox1_Enter(object sender, EventArgs e)
        {
            listBox1.ForeColor = Color.Black;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //Skift fokus fra sidelisten til dokumentliste
            if (e.KeyCode == Keys.Left) //venstre pil
            {
                e.Handled = true;
                shellTreeView1.Focus();
                listBox1.ForeColor = Color.DarkGray;
            }
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            listBox1.ForeColor = Color.DarkGray;
        }


        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                LoadImage(docPageList[listBox1.SelectedIndex].FileName, docPageList[listBox1.SelectedIndex].PageNo);
                if (ImgViewForm1.Visible) ImgViewForm1.rasterImageViewer1.SetImage(rasterPictureBox1);
                if (TagViewForm1.Visible) TagViewForm1.ReadTags(docPageList[listBox1.SelectedIndex].FileName, docPageList[listBox1.SelectedIndex].PageNo);
            }
        }

        /// Indlæser det ønskede dokument
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadImage(string fileName, int pageNr)
        {
            if (File.Exists(fileName))
            {
                //listBox1.SelectedIndex = pageNr - 1; fungerer ikke ved singlepage filer
                if (listBox1.SelectedIndex > -1)
                {
                    var ext = GetFileExtension(fileName);
                    if (ext == "tif")
                        buttonShowTifTags.Enabled = true;
                    else
                        buttonShowTifTags.Enabled = false;

                    rasterPictureBox1.TryLoadPage(docPageList[listBox1.SelectedIndex]);
                }
            }
            else
            {
                MessageBox.Show(fileName + "  ikke fundet!");
            }
        }

        private void LoadImageForPopup(string fileName)
        {
            if (File.Exists(fileName))
            {
                var ext = GetFileExtension(fileName);
                if (ext == "tif")
                    buttonShowTifTags.Enabled = true;
                else
                    buttonShowTifTags.Enabled = false;

                var docPage = new DocPage();
                docPage.FileName = fileName;
                docPage.PageNo = 1;
                rasterPictureBox1.TryLoadPage(docPage);
            }
            else
            {
                MessageBox.Show(fileName + "  ikke fundet!");
            }
        }


        private void OnTick(object sender, EventArgs e)
        {
            if (timerForceStop)
            {
                ti.Stop();
                AutoVisCheckBox.CheckState = CheckState.Unchecked;
                return;
            }

            if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
            {
                ti.Stop();
                listBox1.SelectedIndex++;
                //listBox1_Click(sender,e);//Visning sker indexChange event ikke ved virtuelt click
                Application.DoEvents(); //Sikrer at der ikke kommer noget nyt tick før visning er slut
                ti.Start();
            }
            else
            {
                ti.Stop();
                AutoVisCheckBox.CheckState = CheckState.Unchecked;
            }
        }


        private void populateDocList(string folderName)
        {
            listBox1.Items.Clear();
            docPageList.Clear();

            var docList = new List<string>();


            //***Gennemløb af folders***
            try
            {
                foreach (var fil in Directory.GetFiles(folderName))
                {
                    var ext = Path.GetExtension(fil).ToUpper();
                    if (ext == ".TIF" || ext == ".JP2") docList.Add(fil);
                }
            }
            catch
            {
                //System Volume Folder m.fl.
                MessageBox.Show("Fejl i forb. med opbygning af mappeliste");
            }


            //Sortering således at sidenummerering udfra filnavn sikres
            docList.Sort(sortDocListByPageNo);

            var sideNr = 1; //Dokumentets sidenummer 
            for (var i = 0; i < docList.Count; i++)
            {
                //Hent sideantal
                var pageCount = GetPageCount(docList[i]);


                if (pageCount == -1)
                {
                    MessageBox.Show("Ikke muligt at bestemme sideantal for filen " + docList[i]);
                    listBox1.Items.Clear();
                    return;
                }

                for (var ii = 0; ii < pageCount; ii++)
                {
                    var dp = new DocPage {FileName = docList[i], PageNo = ii + 1};
                    //Her skal filens lokale sidenummer indsættes
                    docPageList.Add(dp);
                    //docList.Add(fil);
                    listBox1.Items.Add("Side " + sideNr.ToString().PadLeft(5, '0'));
                    sideNr++; //Dokumentets overordnede sidenummer
                }
            }
        }


        private void progressBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                if (e.X > 0 && e.X <= 50)
                {
                    progressBar1.Value = e.X;
                    ti.Interval = 5100 - e.X * 100; //  1-50 ~ 100-5000 Default 4000 m/sec
                }
        }


        private void progressBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                if (e.X > 0 && e.X <= 50)
                {
                    progressBar1.Value = e.X;
                    ti.Interval = 5100 - e.X * 100; //  1-50 ~ 100-5000 Default 4000 m/sec
                }
        }

        private void rasterPictureBox_showImage()
        {
            if (rasterPictureBox1.HasImage)
            {
                if (ImgViewForm1.Visible == false) ImgViewForm1.Show();
                ImgViewForm1.rasterImageViewer1.SetImage(rasterPictureBox1);
                //ivf.WindowState = FormWindowState.Maximized;
            }
        }


        private void rasterPictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) rasterPictureBox_showImage();
        }

        public void SetParamValueCallbackFn(string fileName, int pageNo)
        {
            LoadImage(fileName, pageNo);
        }

        private void shellTreeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            //Altid stop dokument auto timer
            ti.Stop();
            AutoVisCheckBox.CheckState = CheckState.Unchecked;


            if (shellTreeView1.SelectedPath != "")
            {
                var path = shellTreeView1.SelectedPath;
                var fas = File.GetAttributes(path);
                if ((fas & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    ShowDocumentFolder(path);
                }
                else
                {
                    var ext = Path.GetExtension(shellTreeView1.SelectedPath).ToUpper();
                    if (ext == ".TIF" || ext == ".JP2") ShowDocumentFile(shellTreeView1.SelectedPath);
                }
            }
        }

        private void shellTreeView1_KeyDown(object sender, KeyEventArgs e)
        {
            //Skift fokus fra shellTreeView til dokumentliste
            if (e.KeyCode == Keys.Enter)
            {
                listBox1.Focus();
                listBox1.ForeColor = Color.Black;
            }
        }


        /// <summary>
        ///     Åbner dokument fil
        /// </summary>
        /// <param name="folderName"></param>
        public void ShowDocumentFile(string fileName)
        {
            listBox1.Items.Clear();
            docPageList.Clear();

            var sideNr = 1; //Dokumentets sidenummer 

            //Hent sideantal
            var pageCount = GetPageCount(fileName);

            if (pageCount == -1)
            {
                MessageBox.Show("Ikke muligt at bestemme sideantal for filen " + fileName);
                return;
            }

            for (var ii = 0; ii < pageCount; ii++)
            {
                var dp = new DocPage {FileName = fileName, PageNo = ii + 1};
                //Her skal filens lokale sidenummer indsættes
                docPageList.Add(dp);
                //docList.Add(fil);
                listBox1.Items.Add("Side " + sideNr.ToString().PadLeft(5, '0'));
                sideNr++; //Dokumentets overordnede sidenummer
            }


            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
                LoadImage(fileName, 1);
                if (ImgViewForm1.Visible) ImgViewForm1.rasterImageViewer1.SetImage(rasterPictureBox1);
                if (TagViewForm1.Visible) TagViewForm1.ReadTags(fileName, 0);
            }

            Update_ShellTree(new FileInfo(fileName).DirectoryName);
        }


        public void ShowDocumentFileForId(int fileNumber)
        {
            var path = shellTreeView1.SelectedPath;
            var folderName = path;
            if (!path.EndsWith("\\" + fileNumber)) folderName = path + "\\" + fileNumber;
            var fileName = "";
            var i = 1;
            while (Directory.Exists(path))
            {
                folderName = path.Split(new[] {"Documents"}, StringSplitOptions.None).First();

                if (folderName.EndsWith("\\"))
                {
                    path = folderName + "Documents\\docCollection" + i;
                    folderName = folderName + "Documents\\docCollection" + i + "\\" + fileNumber;
                }
                else
                {
                    path = folderName + "\\Documents\\docCollection" + i;
                    folderName = folderName + "\\Documents\\docCollection" + i + "\\" + fileNumber;
                }


                if (Directory.Exists(folderName))
                {
                    fileName = folderName + "\\1.tif";
                    if (!File.Exists(fileName))
                    {
                        fileName = folderName + "\\1.jp2";
                        if (!File.Exists(fileName)) return;
                    }

                    populateDocList(folderName);
                    if (TestSettings.Internal_Viewer)
                    {
                        LoadImageForPopup(fileName);
                        rasterPictureBox_showImage();
                    }
                    else
                    {
                        var process = new Process();
                        var startInfo = new ProcessStartInfo();
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        if (docPageList.Count >= 1) fileName = docPageList[0].FileName.Replace(@"\", @"\\");

                        startInfo.Arguments = "/K \"\"" + fileName + "\"\"";
                        process.StartInfo = startInfo;
                        Process.Start(startInfo);
                    }

                    break;
                }

                i++;
            }
        }

        /// <summary>
        ///     Åbner dokument mappe indeholdende en eller flere filer
        /// </summary>
        /// <param name="folderName"></param>
        public void ShowDocumentFolder(string folderName)
        {
            if (Directory.Exists(folderName))
            {
                populateDocList(folderName);

                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0;
                    LoadImage(docPageList[0].FileName, docPageList[0].PageNo);
                    if (ImgViewForm1.Visible) ImgViewForm1.rasterImageViewer1.SetImage(rasterPictureBox1);
                    if (TagViewForm1.Visible) TagViewForm1.ReadTags(docPageList[0].FileName, docPageList[0].PageNo);
                }
            }
        }


        /// <summary>
        ///     Comparison dokumentnummer
        /// </summary>
        /// <param name="fileName1"></param>
        /// <param name="fileName2"></param>
        /// <returns></returns>
        private int sortDocListByPageNo(string fileName1, string fileName2)
        {
            try
            {
                var name1 = Path.GetFileNameWithoutExtension(fileName1);
                var name2 = Path.GetFileNameWithoutExtension(fileName2);

                if (name1 == "") return 0; //Ens fortolkes som fejl
                if (name2 == "") return 0; //Ens fortolkes som fejl

                if (Convert.ToInt16(name1) > Convert.ToInt16(name2))
                    return 1;
                if (Convert.ToInt16(name1) < Convert.ToInt16(name2))
                    return -1;
                return 0; //Ens fortolkes som fejl
            }
            catch
            {
                return 0; //Ens fortolkes som fejl
            }
        }

        public void Update_ShellTree(string path)
        {
            shellTreeView1.SelectedPath = path;
        }

        #endregion
    }
}