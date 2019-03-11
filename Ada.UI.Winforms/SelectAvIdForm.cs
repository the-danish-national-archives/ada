//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//
//namespace Ada.UI.Winforms
//{
//    using System.IO;
//
//    using Ada.Common;
//
//    using Ra.DomainEntities;
//
//    public partial class SelectAvIdForm : Form
//    {
//
//        public AViD ID { get; set; }
//
//        public string IDText { get; set; }
//
//        public SelectAvIdForm()
//        {
//            InitializeComponent();
//        }
//
//        private void SelectAvId_Load(object sender, System.EventArgs e)
//        {
//            textBoxAV.Text = IDText;
//        }
//
//
//        private void BrowseAV_Click(object sender, EventArgs e)
//        {
//            folderBrowserDialog1.SelectedPath = textBoxAV.Text;
//
//            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
//            {
//                if (Directory.Exists(folderBrowserDialog1.SelectedPath))
//                {
//                    textBoxAV.Text = Path.GetFileName(folderBrowserDialog1.SelectedPath);
////                    tabControl.SelectedTab = tabPageSettings; //Vis settings
//                }
//            }
//        }
//
//
//
//        private void StartButton_Click(object sender, EventArgs e)
//        {
//            ID = new AViD(this.textBoxAV.Text);
//            var mediaDirectories = AVMapping.GetMediaDirectories(ID, null);
//            if (mediaDirectories.Count < 1)
//            {
//                MessageBox.Show("Afleveringen eksisterer ikke");
//                return;
//            }
//
//            this.Close();
//
////            if (textBoxAV.Enabled == false)
////            {
////                System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
////                return;
////            }
////
////            if (settings.Drives.getActiveDrives().Count <= 0)
////            {
////                MessageBox.Show("Der er ikke valgt nogle aktive drev ");
////                return;
////            }
////
////
////            LogClear();
////            status.Reset();
////            LogRichTextBox.Focus(); //For at hindre enter i at udløse ny start test
////
////            ID = new AViD(this.textBoxAV.Text);
////            var mediaDirectories = AVMapping.GetMediaDirectories(ID, settings.Drives.getActiveDrives());
////            if (mediaDirectories.Count < 1)
////            {
////                MessageBox.Show("Afleveringen eksisterer ikke");
////                return;
////            }
////
////
////            this.textBoxAV.Enabled = false;
////            browseAV.Enabled = false;
////            tabPageSettings.Enabled = false;
////            gbDeleteDBs.Enabled = false;
////
////            startButton.Text = "Start ny ADA";
////
////            UC_DocumentViewer1.Update_ShellTree(mediaDirectories.First());
////            UC_ContextDocumentViewer1.GetDocumentInfo(mediaDirectories.First(), true, false);
////
////
////
////            var path = new DirectoryInfo(Properties.Settings.Default.DBCreationFolder);
////            var testFactory = new AdaTestUowFactory(this.ID, "test", path);
////            var logFactory = new AdaLogUowFactory(this.ID, "log", path);
////            var avFactory = new AdaAvUowFactory(this.ID, "av", path);
////
////            DialogResult dialogResult;
////            if (logFactory.DataBaseExists())
////                dialogResult = MessageBox.Show(
////                    "Der findes en tidligere kørsel af denne arkiveringsversion, ønsker du at fortsætte med at arbejde på denne?",
////                    "Eksisterende testsæt",
////                    MessageBoxButtons.YesNo);
////            else
////                dialogResult = DialogResult.No;
////
////            if (dialogResult == DialogResult.No)
////            {
////                Task t = Task.Run(
////                    () =>
////                    {
////                        testFactory.DeleteDataBase();
////                        testFactory.CreateDataBase();
////                        using (UnitOfWork uow = (UnitOfWork)testFactory.GetUnitOfWork())
////                        {
////                            using (var cmd = uow.Session.Connection.CreateCommand())
////                            {
////                                cmd.CommandText = Properties.Resources.DbCreate;
////                                cmd.ExecuteNonQuery();
////                            }
////                        }
////                        logFactory.DeleteDataBase();
////                        logFactory.CreateDataBase();
////                        var errorTexts =
////                            new FileInfo(
////                                Path.Combine(
////                                    Path.GetDirectoryName(Application.ExecutablePath) ?? "",
////                                    "errortexts.xml"));
////                        AdaTestLog.LoadErrorTypesFromFile(errorTexts, logFactory);
////                        avFactory.DeleteDataBase();
////                        avFactory.CreateDataBase();
////                    });
////                t.Wait();
////            }
////
////            //Selve testen af AV'en starter
////            this.ucWpf.DataContext = null;
////
////            //settings.Serialize();
////            new Task(() =>
////            Controller.DoFullTest(ID, this.settings)).Start();
////
////            var pmVm = ucPLM.DataContext as ProgressManagementViewModel;
////            if (pmVm != null)
////                pmVm.TestLog = new AdaTestLog(logFactory, Guid.Empty, typeof(MainForm), 0);
////
////
////            this.LogHeader(ID.FullID, Color.Black, true, 14);
////            this.LogHeader(
////                "ADA " + Application.ProductVersion + " på " +
////                System.Security.Principal.WindowsIdentity.GetCurrent().Name,
////                Color.Black,
////                true,
////                9);
////
////            foreach (
////                FieldInfo fi in
////                    settings.GetType()
////                        .GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
////            {
////                object current = fi.GetValue(settings);
////                if (current == null)
////                    continue;
////                PropertyInfo[] piarray =
////                    current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
////
////                bool active = false;
////                bool masterSwitchFound = false;
////
////                if (UILabelsAttribute.hasUILabels(current.GetType()) &&
////                    current.GetType() != typeof(DriveSettings))
////                {
////                    foreach (PropertyInfo pi in piarray)
////                    {
////                        if (UILabelsAttribute.hasUILabels(pi) &&
////                            UILabelsAttribute.GetUIRole(pi) == "Master Switch")
////                        {
////                            masterSwitchFound = true;
////                            active = (bool)pi.GetValue(current, null);
////                        }
////                    }
////
////                    if (!masterSwitchFound || active)
////                    {
////                        this.LogHeader(UILabelsAttribute.GetUIName(current.GetType()), Color.Black, true, 8);
////                        foreach (PropertyInfo pi in piarray)
////                        {
////                            if (UILabelsAttribute.hasUILabels(pi) &&
////                                UILabelsAttribute.GetUIRole(pi) != "Master Switch")
////                            {
////                                this.LogHeader(
////                                    "\t" + UILabelsAttribute.GetUIName(pi) + " " + pi.GetValue(current, null),
////                                    Color.Black,
////                                    true,
////                                    8);
////                            }
////                        }
////                    }
////                    else
////                    {
////                        this.LogHeader(
////                            UILabelsAttribute.GetUIName(current.GetType()) + " testes ikke",
////                            Color.Black,
////                            true,
////                            8);
////                    }
////                }
////            }
//        }
//
//
//        private void textBoxAV_KeyUp(object sender, KeyEventArgs e)
//        {
//            if (e.KeyValue == 13)
//            {
////                LogRichTextBox.Focus();
//                startButton.PerformClick();
//            }
//        }
//
//    }
//}

