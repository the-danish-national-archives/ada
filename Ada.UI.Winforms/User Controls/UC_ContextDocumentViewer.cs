namespace Ada.UI.Winforms.User_Controls
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Schema;
    using Printing;
    using Ra.DocumentInvestigator.OldForWinforms;

    #endregion

    public partial class UC_ContextDocumentViewer : UserControl
    {
        #region  Fields

        private List<DocCollectionIndexInfo> docCollectionIndexInfoList = new List<DocCollectionIndexInfo>();


        /// <summary>
        ///     Liste med dokument sider (en eller flere filer som hver især kan have en eller flere sider)
        /// </summary>
        private readonly List<DocPage> docPageList = new List<DocPage>();

        private readonly UC_DocumentViewerForm ImgViewForm1 = new UC_DocumentViewerForm();

        //Parsning
        private string parserErrorMsg = "";
        private readonly UC_DocumentViewerTagForm TagViewForm1 = new UC_DocumentViewerTagForm();

        //Autovisning af dokument
        private readonly Timer ti = new Timer();
        private bool timerForceStop;

        #endregion

        #region  Constructors

        //Initialisering
        public UC_ContextDocumentViewer()
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
                ti.Interval = 5100 - progressBar1.Value * 100; // 1-50 ~ 100-5000 Default 4000 m/sec
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


        private void button1_Click(object sender, EventArgs e)
        {
            //!!!! Knap skal evt væk i ADA kontext !!!!  René

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) GetDocumentInfo(folderBrowserDialog1.SelectedPath, true, true); //Skal parses før visning og fejlmeldinger vises
            //Slut
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                var docPrint = new SA_DocumentPrint();
                docPrint.PrintDocument(docPageList);
            }
        }


        //private void buttonZoomIn_Click(object sender, EventArgs e)
        //{

        //    //if (rasterPictureBox1. .Image.Sca .ScaleFactor < 10)
        //    //{
        //    //    double f = rasterImageViewer1.ScaleFactor + 1;
        //    //    rasterImageViewer1.ScaleFactor = f;
        //    //}
        //}

        //private void buttonZoomOut_Click(object sender, EventArgs e)
        //{
        //    //if (rasterImageViewer1.ScaleFactor > 1.0)
        //    //{
        //    //    double f = rasterImageViewer1.ScaleFactor - 1;
        //    //    rasterImageViewer1.ScaleFactor = f;
        //    //}
        //    //else
        //    //    if (rasterImageViewer1.ScaleFactor > 0.1)
        //    //    {
        //    //        double f = 0.1;
        //    //        rasterImageViewer1.ScaleFactor = f;
        //    //    }
        //}

        //private void buttonRoterRight_Click(object sender, EventArgs e)
        //{
        //    //Fysisk transformering af fil
        //    //Leadtools.Codecs.CodecsTransformFlags rt = new CodecsTransformFlags();
        //    //rt = CodecsTransformFlags.Rotate90;
        //    //Leadtools.Codecs.RasterCodecs rc = new RasterCodecs();
        //    //rc.Transform(????);


        //    //Hurtig horisontal eller vertikal flip
        //    //Leadtools.ImageProcessing.FlipCommand fc = new Leadtools.ImageProcessing.FlipCommand();
        //    //fc.Horizontal = false;
        //    //fc.Run(rasterImageViewer1.Image);

        //    //Langsom fordi det er bitmap som flyttes rundt !!
        //        //Rotate the image by 45 degrees 
        //    Leadtools.ImageProcessing.RotateCommand command = new Leadtools.ImageProcessing.RotateCommand();
        //    command.Angle = 90 * 100;
        //    command.FillColor = new RasterColor(255, 255, 255);
        //    command.Flags = Leadtools.ImageProcessing.RotateCommandFlags.Bicubic;
        //    command.Run(rasterPictureBox1.Image); 
        //}


        private void buttonShowTifTags_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (TagViewForm1.Visible == false) TagViewForm1.Show();
                TagViewForm1.ReadTags(docPageList[listBox1.SelectedIndex].FileName, docPageList[listBox1.SelectedIndex].PageNo);
            }
        }

        private void docListBox_Click(object sender, EventArgs e)
        {
            docListBox_selectDocument(docListBox.SelectedIndex); //Svarer til klik
        }

        private void docListBox_Enter(object sender, EventArgs e)
        {
            docListBox.ForeColor = Color.Black;
            listBox1.ForeColor = Color.DarkGray;
        }


        private void docListBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Skift fokus fra dokumentliste til sideliste
            if (e.KeyCode == Keys.Right) //højre pil
            {
                e.Handled = true;
                listBox1.Focus();
                listBox1.ForeColor = Color.Black;
                docListBox.ForeColor = Color.DarkGray;
            }

            if (e.KeyCode == Keys.Enter) docListBox_selectDocument(docListBox.SelectedIndex); //Svarer til klik
        }

        private void docListBox_Leave(object sender, EventArgs e)
        {
            docListBox.ForeColor = Color.DarkGray;
        }


        //Fælles metode for docListBox_Click og docListBox_KeyUp
        private void docListBox_selectDocument(int index)
        {
            if (docListBox.SelectedIndex > -1)
            {
                var docPath = Path.GetDirectoryName(docCollectionIndexInfoList[docListBox.SelectedIndex].fullPath);
                ShowDocumentFolder(docPath);
                richTextBox1.Text = docCollectionIndexInfoList[docListBox.SelectedIndex].desc;
            }
        }

        /// <summary>
        ///     Benyttes til at indlæse contextDocumentation informationer.
        ///     silentFail benyttes til at undertrykke evt. fejlmeldinger ved forhåndsindlæsning når test startes
        /// </summary>
        /// <param name="AV"></param>
        public void GetDocumentInfo(string firstMediaId, bool parse, bool showErrors)
        {
            docListBox.Items.Clear();
            richTextBox1.Clear();

            //Led efter contextDocumentationIndex.xml
            var XMLpath = "";

            var folderName = Path.GetFileName(firstMediaId);

            if (folderName.Contains("AVID"))
                XMLpath = firstMediaId + "\\Indices\\contextDocumentationIndex.xml";
            else if (folderName == "Indices")
                XMLpath = firstMediaId + "\\contextDocumentationIndex.xml";
            else if (folderName == "ContextDocumentation")
                XMLpath = firstMediaId.Substring(0, firstMediaId.Length - (folderName.Length + 1)) + "\\Indices\\contextDocumentationIndex.xml";
            else if (folderName == "Documents")
                XMLpath = firstMediaId.Substring(0, firstMediaId.Length - (folderName.Length + 1)) + "\\Indices\\contextDocumentationIndex.xml";
            else if (folderName == "Schemas")
                XMLpath = firstMediaId.Substring(0, firstMediaId.Length - (folderName.Length + 1)) + "\\Indices\\contextDocumentationIndex.xml";
            else if (folderName == "Tables") XMLpath = firstMediaId.Substring(0, firstMediaId.Length - (folderName.Length + 1)) + "\\Indices\\contextDocumentationIndex.xml";


            if (File.Exists(XMLpath) == false)
            {
                if (showErrors) MessageBox.Show("contextDocumentationIndex.xml ikke fundet!");
                return;
            }


            var stream = new FileStream(XMLpath, FileMode.Open, FileAccess.Read);
            var reader = new XmlTextReader(stream);
            reader.WhitespaceHandling = WhitespaceHandling.All;
            reader.Namespaces = true;
            var doc = new XmlDocument();
            try
            {
                doc.Load(reader);
            }
            catch
            {
                if (showErrors)
                    MessageBox.Show("contextDocumentation.xml fil ikke velformed!" + "\n" +
                                    "Kan ikke vise contextDocumentation dokumenter.");
                return;
            }

            parserErrorMsg = "";
            if (parse)
            {
                var schemaPath = XMLpath.Replace("\\Indices\\contextDocumentationIndex.xml", "\\Schemas\\standard\\contextDocumentationIndex.xsd");
                if (File.Exists(schemaPath) == false)
                {
                    if (showErrors) MessageBox.Show("Schema contextDocumentationIndex.xsd ikke fundet!");
                    return;
                }

                var schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, schemaPath);

                doc.Schemas.Add(schemaSet);
                ValidationEventHandler eventHandler = ValidationEventHandler;

                try
                {
                    doc.Validate(eventHandler);
                }
                catch (XmlSchemaException e)
                {
                    if (showErrors) MessageBox.Show("Fejl! " + e.Message);
                }
            } //parse ...

            if (parserErrorMsg.Length > 0)
            {
                if (showErrors) MessageBox.Show("FEJL! kan ikke vise contextDocumentation dokumenter: \n" + parserErrorMsg);
                return;
            }

            var docPath = XMLpath.Replace("\\Indices\\contextDocumentationIndex.xml", "\\ContextDocumentation\\docCollection1\\");
            var nodes = doc.GetElementsByTagName("document");
            docCollectionIndexInfoList = new List<DocCollectionIndexInfo>();
            foreach (XmlNode node in nodes)
            {
                var docID = node.ChildNodes[0].FirstChild.Value;
                var name = node.ChildNodes[1].FirstChild.Value;


                //!!! findes der en smartere måde? !!!
                var pathToDocindexFile = "";
                if (File.Exists(docPath + docID + "\\1.tif"))
                    pathToDocindexFile = docPath + docID + "\\1.tif";
                else if (File.Exists(docPath + docID + "\\1.mpg"))
                    pathToDocindexFile = docPath + docID + "\\1.mpg";
                else if (File.Exists(docPath + docID + "\\1.jp2"))
                    pathToDocindexFile = docPath + docID + "\\1.jp2";
                else if (File.Exists(docPath + docID + "\\1.mp3"))
                    pathToDocindexFile = docPath + docID + "\\1.mp3";


                var dcii = new DocCollectionIndexInfo();
                if (!File.Exists(pathToDocindexFile))
                {
                    if (showErrors) MessageBox.Show("Der mangler en contextDocumentations fil " + docID + " " + name);
                    return;
                }

                dcii.docID = docID;
                dcii.name = name;
                dcii.fullPath = pathToDocindexFile;
                if (!string.IsNullOrEmpty(node.ChildNodes[2].FirstChild.Value))
                    dcii.desc = node.ChildNodes[2].FirstChild.Value;
                else
                    dcii.desc = "";
                docCollectionIndexInfoList.Add(dcii);
            }

            reader.Close();


            //Herefter min kode som benyttes når Tommy har rettet sin til ???
            foreach (var info in docCollectionIndexInfoList) docListBox.Items.Add("(" + info.docID + ") " + info.name);
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
            var tiffDll = new TiffLeadToolsProcessor();
            return tiffDll.GetPageCount(fil);
        }


        private void listBox1_Enter(object sender, EventArgs e)
        {
            listBox1.ForeColor = Color.Black;
            docListBox.ForeColor = Color.DarkGray;
        }


        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //Skift fokus fra sidelisten til dokumentliste
            if (e.KeyCode == Keys.Left) //venstre pil
            {
                e.Handled = true;
                docListBox.Focus();
                docListBox.ForeColor = Color.Black;
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
                //listBox1.SelectedIndex = pageNr - 1;//fungerer ikke med singlepage filer
                if (listBox1.SelectedIndex > -1)
                {
                    var ext = GetFileExtension(fileName);
                    if (ext == "tif")
                        buttonShowTifTags.Enabled = true;
                    else
                        buttonShowTifTags.Enabled = false;
                    rasterPictureBox1.LoadPage(docPageList[listBox1.SelectedIndex]);
                }
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


        private void rasterPictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (rasterPictureBox1.HasImage)
                {
                    if (ImgViewForm1.Visible == false)
                        ImgViewForm1.Show();
                    ImgViewForm1.rasterImageViewer1.SetImage(rasterPictureBox1);
                    //ivf.WindowState = FormWindowState.Maximized;
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
                var dp = new DocPage();
                dp.FileName = fileName;
                dp.PageNo = ii + 1; //Her skal filens lokale sidenummer indsættes
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

        /// <summary>
        ///     Eventhandler som håndterer de fejl som kommer i forbindelsen med valideringen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    parserErrorMsg += "Error: " + e.Message + "\n";
                    break;
                case XmlSeverityType.Warning:
                    parserErrorMsg += "Warning: " + e.Message + "\n";
                    break;
            }
        }

        #endregion

        #region Nested type: DocCollectionIndexInfo

        public class DocCollectionIndexInfo
        {
            #region  Fields

            public string desc;
            public string docID;
            public string fullPath;
            public string name;

            #endregion
        }

        #endregion
    }
}