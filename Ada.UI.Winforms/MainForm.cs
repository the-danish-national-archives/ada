namespace Ada.UI.Winforms
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;
    using Common;
    using Core;
    using Log;
    using Printing;
    using Properties;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Reflection;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;
    using Ra.DomainEntities.TableIndex;
    using Reflection;
    using Repositories;
    using SA;
    using User_Controls;
    using Wpf.ProgressManagement.ViewModel;
    using Wpf.TableIndexViewer.ViewModel;
    using Wpf.TableIndexViewer.Views;
    using Wpf.WorkspaceCleanUp.ViewModel;

    #endregion

    [DesignerCategory("")]
    public partial class MainForm : Form
    {
        #region  Fields

        private AdaAvUowFactory avFactory;
        private AViD ID;
        private AdaLogUowFactory logFactory;
        private readonly int minSplitterDist = 46;
        private readonly Dictionary<string, int> RestrictedEvents = new Dictionary<string, int>();

        private bool running;

        private bool scrollDelayIsIn;
        private bool scrollDelaySkipped;
        public TestSettings settings = new TestSettings();


        private bool splitContainerRight_SplitterMoved_inside;
        private readonly AVStatus status = new AVStatus();

        private AdaTestUowFactory testFactory;

        #endregion

        #region  Constructors

        //        private string AvId;

        public MainForm()
        {
            InitializeComponent();

            SafeInvoker.SetSyncContext(this);

            Controller.Initialize();
            Controller.LoggingEvent += Director_LoggingEvent;
            Controller.LoggingEvent += OnActions;
            AdaTestLog.LoggingEvent += WriteLogOutput;

            status.Dispatcher = this;

            Controller.LoggingEvent += status.LoggingEvent;
            AdaTestLog.LoggingEvent += status.LoggingEvent;


            ucWCU.DataContext = new WorkspaceCleanUpViewModel();
            ThisForm = this;
        }

        public MainForm(TestSettings settings, string AvId) : this()
        {
            this.settings = settings;
            textBoxAV.Text = AvId;

            ID = new AViD(textBoxAV.Text);

            var mediaDirectories = AVMapping.GetMediaDirectories(ID, settings.Drives.GetActiveDrives());
//            splitContainerMain.Panel1Collapsed = true;
            var firstMedia = mediaDirectories.FirstOrDefault();
            if (firstMedia != null)
            {
                UC_DocumentViewer1.Update_ShellTree(firstMedia);
                UC_ContextDocumentViewer1.GetDocumentInfo(firstMedia, true, false);
            }
        }

        #endregion

        #region Properties

        public ElementHost TableViewerTab { get; set; }
        public ElementHost TableViewerTabPopup { get; set; }
        public PopupWindow TableViewForm { get; set; }
        public bool TableViewFormFloating { get; set; }
        public static Form ThisForm { get; set; }

        public TableIndexViewer uucTIV { get; set; }

        #endregion

        #region

        private void AddStatusGroupBox()
        {
            //Obpyg statusområde
            var groupBoxStatus = new GroupBox();
            groupBoxStatus.Text = "Status";
            groupBoxStatus.Location = new Point(7, 100);
            groupBoxStatus.Height = 220;
            groupBoxStatus.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            groupBoxStatus.Width = splitContainerMain.Panel1.Width - 7;


            BuildAVStatus(groupBoxStatus, status);

            splitContainerMain.Panel1.Controls.Add(groupBoxStatus);
        }

        private static void AddTestStatus(Control parent, int verticalOffset, TestStatus bindingObject, string text)
        {
            var picBoxStatus = new PictureBox();
            var labelTestName = new Label();
            var picBoxBusy = new PictureBox();
            var labelProgress = new Label();

            picBoxBusy.Size = new Size(16, 16);
            picBoxStatus.Size = new Size(16, 16);
            labelTestName.Size = new Size(155, 16);
            labelProgress.Size = new Size(150, 16);

//            labelProgress.Parent = labelTestName;
//            labelProgress.BackColor = Color.Transparent;
            labelProgress.TextAlign = ContentAlignment.TopRight;

            picBoxStatus.Location = new Point(12, verticalOffset);
            picBoxBusy.Location = new Point(parent.Width - (5 + 16), verticalOffset);
            labelTestName.Location = new Point(34, verticalOffset + 3);
            labelProgress.Location = new Point(parent.Width - (5 + 5 + 16 + 150), verticalOffset + 3);


            picBoxStatus.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            picBoxBusy.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            labelProgress.Anchor = AnchorStyles.Right | AnchorStyles.Top;


            picBoxStatus.DataBindings.Add("Image", bindingObject, "StatusImage");
            picBoxStatus.Tag = bindingObject;


            labelTestName.Text = text;
            labelProgress.DataBindings.Add("Text", bindingObject, "Progress");
            picBoxBusy.DataBindings.Add("Image", bindingObject, "BusyImage");

            labelTestName.AutoSize = true;

            parent.Controls.Add(picBoxStatus);
            parent.Controls.Add(picBoxBusy);
            parent.Controls.Add(labelTestName);
            parent.Controls.Add(labelProgress);
            parent.Height = verticalOffset + 24;
        }


        private void BrowseAV_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBoxAV.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;
                if (Directory.Exists(path)) SelectPath(path);
            }
        }

        private static void BuildAVStatus(Control parent, AVStatus status)
        {
            var verticalOffset = 24;
            var verticalSubOffset = 24;
            var currentgroup = "";
            var box = new GroupBox();

            foreach (
                var PI in
                status.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                if (UILabelsAttribute.hasUILabels(PI))
                {
                    if (currentgroup != UILabelsAttribute.GetUIGrouping(PI))
                    {
                        currentgroup = UILabelsAttribute.GetUIGrouping(PI);
                        box = new GroupBox();
                        box.Text = currentgroup;
                        box.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                        parent.Controls.Add(box);
                        box.Location = new Point(12, verticalOffset);
                        box.Height = 48;
                        box.Width = parent.Width - 24;

                        verticalSubOffset = 20;
                        verticalOffset += 30;
                    }

                    AddTestStatus(box, verticalSubOffset, PI.GetValue(status, null) as TestStatus,
                        UILabelsAttribute.GetUIName(PI));
                    verticalSubOffset += 22;
                    verticalOffset += 22;
                }

            parent.Height = verticalOffset;
        }

        private void butInspec1_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            if (button.Tag as string != "collapsed")
            {
                splitContainerRight.Tag = splitContainerRight.SplitterDistance;
                splitContainerRight.SplitterDistance = minSplitterDist;
                button.Image = Resources.expand;
                button.Tag = "collapsed";
            }
            else
            {
                if (splitContainerRight.Tag != null)
                    splitContainerRight.SplitterDistance = (int) splitContainerRight.Tag;
                else
                    splitContainerRight.SplitterDistance = 300; //Defaultværdi
                splitContainerRight.Tag = null;
                button.Image = Resources.collapse;
                button.Tag = "expand";
            }
        }


        private void buttonCreateFileIndex_Click(object sender, EventArgs e)
        {
            writeFileIndex(false);
        }

        private void buttonOpenArchiveIndex_Click(object sender, EventArgs e)
        {
            openIndexFile("archiveIndex.xml");
        }

        private void buttonOpenContextDocumentationIndex_Click(object sender, EventArgs e)
        {
            openIndexFile("contextDocumentationIndex.xml");
        }

        private void buttonOpenDocIndex_Click(object sender, EventArgs e)
        {
            openIndexFile("docIndex.xml");
        }

        private void buttonOpenFileIndex_Click(object sender, EventArgs e)
        {
            openIndexFile("fileIndex.xml");
        }

        private void buttonOpenTableIndex_Click(object sender, EventArgs e)
        {
            openIndexFile("tableIndex.xml");
        }

        private void buttonPrintLog_Click(object sender, EventArgs e)
        {
            var sap = new SA_DocumentPrint();
            sap.PrintTekst(LogRichTextBox);
        }

        private void buttonSaveLogAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = textBoxAV.Text + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" +
                                       DateTime.Now.Day + ".rtf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) LogRichTextBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
        }

        private void buttonUpdateFileIndex_Click(object sender, EventArgs e)
        {
            writeFileIndex(true);
        }


        private void ClearDataContexts()
        {
            void TryDispose(object obj)
            {
                IDisposable dis;
                dis = obj as IDisposable;
                dis?.Dispose();
                obj = null;
            }

            TryDispose(ucTIV.DataContext);
            ucTIV.DataContext = null;
            TryDispose(uucTIV.DataContext);
            uucTIV.DataContext = null;

            TryDispose(ucCUC.DataContext);
            ucCUC.DataContext = null;

            TryDispose(ucPLM.DataContext);
            ucPLM.DataContext = null;

            var ucPLMDataContext = new ProgressManagementViewModel();
            ucPLMDataContext.RunTestRequested += UcPLMDataContext_RunTestRequested;
            ucPLM.DataContext = ucPLMDataContext;
            GC.Collect(3, GCCollectionMode.Forced, true);
        }

        //****** Log Event ******
        private void Director_LoggingEvent(object sender, LoggingEventArgs loggingEvent)
        {
            if (loggingEvent.EventType == LoggingEventType.ProgressUpdate)
                return;

            if (!string.IsNullOrEmpty(loggingEvent.EventMessage))
            {
                var text = loggingEvent.EventMessage;
                var color = Color.Black;


                switch (loggingEvent.EventType)
                {
                    case LoggingEventType.Info:
                        color = Color.FromArgb(110, 110, 110);
                        text = "\n" + text;
                        break;
                    case LoggingEventType.Error:
                        color = Color.FromArgb(160, 20, 20);
                        text = "\n" + text;
                        break;
                    case LoggingEventType.TestSkippedPreConditionsNotMet:
                        color = Color.Black;
                        text = "\n" + text;
                        break;
                    case LoggingEventType.TestEnd:
                        color = Color.Black;
                        text = "\n" + text;
                        break;
                }

                SafeAddText(text, color);
            }
        }

        private void EnableTableIndex()
        {
            ucTIV.Dispatcher.BeginInvoke(
                (Action) (() =>
                {
                    var uowTest = testFactory?.GetUnitOfWork();
                    if (uowTest == null)
                        return;

                    var repository = uowTest.GetRepository<TableIndex>();
                    var tableIndex = repository.All().FirstOrDefault();

                    if (tableIndex == null)
                        return;

                    var uowAv = (UnitOfWork) avFactory.GetUnitOfWork();


                    var dataContext =
                        new TableIndexViewerViewModel(
                            tableIndex,
                            uowAv.Session.Connection);
                    ucTIV.DataContext = dataContext;
                    var bindingsFromProgress = (ucPLM.DataContext as ProgressManagementViewModel)?.CommandBindings;
                    if (bindingsFromProgress != null)
                        ucTIV.CommandBindings.AddRange(bindingsFromProgress);
                    ucPLM.CommandBindings.AddRange(dataContext.CommandBindings);
                    dataContext.SqlShowViewModel.QuerySelectorViewModel.QuerySet +=
                        (o, e) => tabControl.SelectTab(tabPageTableViewWpf);
                }));
            uucTIV.Dispatcher.BeginInvoke(
                (Action) (() =>
                {
                    var uowTest = testFactory?.GetUnitOfWork();
                    if (uowTest == null)
                        return;

                    var repository = uowTest.GetRepository<TableIndex>();
                    var tableIndex = repository.All().FirstOrDefault();

                    if (tableIndex == null)
                        return;

                    var uowAv = (UnitOfWork) avFactory.GetUnitOfWork();


                    var dataContext =
                        new TableIndexViewerViewModel(
                            tableIndex,
                            uowAv.Session.Connection);
                    uucTIV.DataContext = dataContext;
                    var bindingsFromProgress = (ucPLM.DataContext as ProgressManagementViewModel)?.CommandBindings;
                    if (bindingsFromProgress != null)
                        uucTIV.CommandBindings.AddRange(bindingsFromProgress);
                    ucPLM.CommandBindings.AddRange(dataContext.CommandBindings);
                    dataContext.SqlShowViewModel.QuerySelectorViewModel.QuerySet +=
                        (o, e) => tabControl.SelectTab(tabPageTableViewWpf);
                }));
        }

        /// <summary>
        ///     Returnerer Extension som LowerCase uden adskilletegn fx "tif"
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFileExtension(string file)
        {
            var ext = Path.GetExtension(file ?? "").ToLower();
            if (ext[0].ToString() == ".")
                return ext.Remove(0, 1); //Fjern dot
            return ext;
            //Fjern dot
        }

        /// <summary>
        ///     Midlertidig implementation af LOG
        /// </summary>
        private void Log(string txt, Color color, bool bold)
        {
            //            LogRichTextBox.SelectionStart = LogRichTextBox.TextLength;
            LogRichTextBox.AppendText(txt + "\n");

            if (txt.Length > 0)
            {
                LogRichTextBox.SelectionLength = txt.Length + 1;
                //                LogRichTextBox.Select(LogRichTextBox.Text.Length - (txt.Length + 1), txt.Length);
                LogRichTextBox.Select(LogRichTextBox.TextLength - (txt.Length + 1), txt.Length);
                if (bold)
                    LogRichTextBox.SelectionFont = new Font("Microsoft Sans Serif", LogRichTextBox.Font.Size,
                        FontStyle.Bold);
                LogRichTextBox.SelectionColor = color;
            }

            ScrollDelay();
        }


        /// <summary>
        ///     Midlertidig implementation af LOG
        /// </summary>
        private void LogClear()
        {
            LogRichTextBox.Clear();
        }

        private void LogHeader(string txt, Color color, bool bold, float size)
        {
            LogRichTextBox.AppendText(txt + "\n\n");

            if (txt.Length > 0)
            {
                LogRichTextBox.Select(LogRichTextBox.Text.Length - (txt.Length + 2), txt.Length);
                if (bold) LogRichTextBox.SelectionFont = new Font("Microsoft Sans Serif", size, FontStyle.Bold);
                LogRichTextBox.SelectionColor = color;
            }

            LogRichTextBox.ScrollToCaret();
        }


        private void LogRichTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //find linie som der er klikket på
            var lineNr = LogRichTextBox.GetLineFromCharIndex(LogRichTextBox.GetCharIndexFromPosition(e.Location));
            var line = LogRichTextBox.Lines[lineNr];

            var parts = line.Split('*');

            if (parts.Length > 1)
            {
                var secondPart = parts[1].Trim();


                var mediaDirectories = AVMapping.GetMediaDirectories(ID, settings.Drives.GetActiveDrives());

                foreach (var prepath in "".Yield().Union(settings.Drives.GetActiveDrives()).Union(mediaDirectories))
                {
                    if (prepath != "")
                        secondPart = secondPart.TrimStart('\\', '/');
                    var path = Path.Combine(prepath, secondPart);
                    //Hvis fil ...
                    if (File.Exists(path))
                    {
                        var ext = GetFileExtension(path).ToLower();

                        switch (ext)
                        {
                            case "tif":
                            case "jp2":
                                tabControl.SelectedTab = tabPageDokument;
                                UC_DocumentViewer1.ShowDocumentFile(path);
                                break;
                            case "wav":
                            case "mpg":
                            case "mp3":
                            case "mp4":
                            default:

                                var windir = Environment.GetEnvironmentVariable("WINDIR");
                                var prc = new Process();
                                prc.StartInfo.FileName = windir + @"\explorer.exe";
                                prc.StartInfo.Arguments = "/select, \"" + path + "\"";
                                prc.Start();

                                //tabControl.SelectedTab = tabPageMediaplayer;
                                //UC_Media_player1.MediaOpen(path);
                                break;
                        }

                        break;
                    }

                    if (Directory.Exists(path))
                    {
                        var windir = Environment.GetEnvironmentVariable("WINDIR");
                        var prc = new Process();
                        prc.StartInfo.FileName = windir + @"\explorer.exe";
                        prc.StartInfo.Arguments = path;
                        prc.Start();
                        break;
                    }
                }
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
#if DEBUG
#else
            var result = MessageBox.Show("Er du sikker på du vil lukke ADA?", "Lukning af ADA",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

            e.Cancel = (result == DialogResult.No);
#endif
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateWindowTitle();
            WindowState = FormWindowState.Maximized;
            Refresh(); //Mainform resizes før komponenter tilføjes
            SuspendLayout();

            RewriteSettingsTabPage();

            AddStatusGroupBox();


            Refresh();


            //*** Initialisering af WebBrowser (Hjælp) ****
            var UrlList = new Dictionary<string, string>();

            string path;
            path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? "", "Brugervejledning.mht");
            UrlList.Add("Ada brugervejledning", path);

            path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? "", "bek1007.mht");
            UrlList.Add("Bekendtgørelse 1007", path);

            UC_WebBrowser1.LoadLinkList(UrlList, false);


            ResumeLayout();
        }

        private void OnActions(object sender, LoggingEventArgs loggingevent)
        {
            if (loggingevent.Source.Name == "TableIngestPresenterAction")
                if (loggingevent.EventType.IsStoppingEvent())
                    EnableTableIndex();
        }

        public void OnTestEnd()
        {
            Invoke(
                (Action) (() =>
                {
//                    buttonRewriteFileIndex.Enabled = true;
//                    buttonRewriteTableXsds.Enabled = true;

                    running = false;
                    tabPageSettings.Enabled = true;
                    UpdateWindowTitle();

                    LogHeader("ADA test afsluttet", Color.Black, true, 14);
                    startButton.Enabled = true;
                }));

//            EnableTableIndex();

            Invoke(
                (Action) (() => { ucCUC.DataContext = new CleanUpCurrentViewModel(textBoxAV.Text); }));
        }

        public void OpenDocumentPopup()
        {
            var ucDocumentChooser = new UC_DocumentChooser(UC_DocumentViewer1);
            ucDocumentChooser.ShowDialog();
        }


//        private void buttonRewriteFileIndex_Click(object sender, EventArgs e)
//        {
//            this.Enabled = false;
//            this.Cursor = Cursors.WaitCursor;
//            try
//            {
//                Controller.RewriteFileIndex();
//            }
//            finally
//            {
//                this.Cursor = Cursors.Default;
//                this.Enabled = true;
//            }
//        }
//
//        private void buttonRewriteTableXsds_Click(object sender, EventArgs e)
//        {
//            this.Enabled = false;
//            this.Cursor = Cursors.WaitCursor;
//            try
//            {
//                Controller.RewriteXsd();
//            }
//            finally
//            {
//                this.Cursor = Cursors.Default;
//                this.Enabled = true;
//            }
//        }


        private void openIndexFile(string fileName)
        {
            if (textBoxAV.Text.Length > 5) // TODO REM hack !!!
            {
                var av = new AViD(textBoxAV.Text);

                var whiteList = new List<string>(settings.Drives.GetActiveDrives());

                if (string.IsNullOrWhiteSpace(av.AVSerial))
                {
                    if (whiteList.Count == 0)
                    {
                        MessageBox.Show("Der er ikke angivet arkiveringsversionsnummer eller 'aktive drev'.");
                        return;
                    }

                    MessageBox.Show("Der er ikke angivet arkiveringsversionsnummer.");
                    return;
                }

                if (whiteList.Count == 0)
                {
                    MessageBox.Show("Der er ikke angivet 'aktive drev'.");
                    return;
                }

                var mapping = AVMapping.CreateMapping(av, whiteList);

                var driveToFirstMediaID = mapping?.GetMediaRoot(1);

                if (driveToFirstMediaID == null)
                {
                    MessageBox.Show("Førte media ikke fundet.");
                    return;
                }

                var fullPath = Path.Combine(driveToFirstMediaID, av.FullID + @".1\Indices\") + fileName;

                if (File.Exists(fullPath))
                {
                    var p = new Process();
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.FileName = fullPath;
                    p.StartInfo.UseShellExecute = true;
                    p.Start();
                }
                else
                {
                    MessageBox.Show("Indexfil " + fullPath + " ikke fundet!");
                }
            }
        }

        private void PrintRunSettingsToLogView()
        {
            LogRichTextBox.Clear();
            LogHeader(ID.FullID, Color.Black, true, 14);
            LogHeader(
                "ADA " + Application.ProductVersion + " på " +
                WindowsIdentity.GetCurrent().Name,
                Color.Black,
                true,
                9);

            foreach (
                var fi in
                settings.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                var current = fi.GetValue(settings);
                if (current == null)
                    continue;
                var piarray =
                    current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var active = false;
                var masterSwitchFound = false;

                if (UILabelsAttribute.hasUILabels(current.GetType()) &&
                    current.GetType() != typeof(DriveSettings))
                {
                    foreach (var pi in piarray)
                        if (UILabelsAttribute.hasUILabels(pi) &&
                            UILabelsAttribute.GetUIRole(pi) == "Master Switch")
                        {
                            masterSwitchFound = true;
                            active = (bool) pi.GetValue(current, null);
                        }

                    if (!masterSwitchFound || active)
                    {
                        LogHeader(UILabelsAttribute.GetUIName(current.GetType()), Color.Black, true, 8);
                        foreach (var pi in piarray)
                            if (UILabelsAttribute.hasUILabels(pi) &&
                                UILabelsAttribute.GetUIRole(pi) != "Master Switch")
                                LogHeader(
                                    "\t" + UILabelsAttribute.GetUIName(pi) + " " + pi.GetValue(current, null),
                                    Color.Black,
                                    true,
                                    8);
                    }
                    else
                    {
                        LogHeader(
                            UILabelsAttribute.GetUIName(current.GetType()) + " testes ikke",
                            Color.Black,
                            true,
                            8);
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Processes a command key - catch the Ctrl + D (Open a document).
        /// </summary>
        /// <param name="msg">
        ///     A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32
        ///     message to process.
        /// </param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>
        ///     <see langword="true" /> if the keystroke was processed and consumed by the control; otherwise,
        ///     <see langword="false" /> to allow further processing.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.D:
                    OpenDocumentPopup();
                    return true;
                case Keys.Control | Keys.T:
                    ToggleFloatTableViewer();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RewriteSettingsTabPage()
        {
// update disk status
            var settingsTabPage = tabControl.TabPages["tabPageSettings"];
            settingsTabPage.SuspendLayout();
            settingsTabPage.Controls.Clear();

            ReflectedGUIBuilder.PopulateSettingsControl(settings, settingsTabPage, true,
                false
//                true
            );

            settingsTabPage.ResumeLayout(true);
        }

        private void SafeAddText(string text, Color color)
        {
            BeginInvoke(new Action(() => { Log("\t" + text, color, true); }));
        }

        private void ScrollDelay()
        {
            if (scrollDelayIsIn)
            {
                scrollDelaySkipped = true;
            }
            else
            {
                scrollDelayIsIn = true;
                Task.Delay(new TimeSpan(0, 0, 0, 0, 100)).ContinueWith(
                    t => { BeginInvoke(new Action(ScrollDelayWorking)); }
                );
            }
        }

        private void ScrollDelayWorking()
        {
            LogRichTextBox.ScrollToCaret();

            var toRepeat = scrollDelaySkipped;
            scrollDelaySkipped = false;
            scrollDelayIsIn = false;
            if (toRepeat)
                ScrollDelay();
        }

        private void SelectPath(string path)
        {
            var root = Path.GetPathRoot(path);
            settings.Drives.UpdateDrives();
            var driveStatus = settings.Drives.DriveList.FirstOrDefault(d => d.Drive.StartsWith(root));
            if (driveStatus != null) driveStatus.Status = true;

            textBoxAV.Text = Path.GetFileName(path);

            RewriteSettingsTabPage();

            tabControl.SelectedTab = tabPageSettings; //Vis settings
        }

        private void splitContainerRight_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainerRight_SplitterMoved_inside)
                return;
            try
            {
                splitContainerRight_SplitterMoved_inside = true;
                //Hvis log vindue kommer under en vis højde lukkes helt, svarende til at bruger klikker på
                //collapse knappen
                if (e.SplitY < 240)
                {
                    splitContainerRight.SplitterDistance = minSplitterDist;

                    butInspec1.Image = Resources.expand;
                    butInspec1.Tag = "collapsed";
                    splitContainerRight.Tag = null;
                }
                else
                {
                    butInspec1.Image = Resources.collapse;
                    butInspec1.Tag = "expand";
                }
            }
            finally
            {
                splitContainerRight_SplitterMoved_inside = false;
            }
        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            if (running)
            {
                MessageBox.Show(
                    "Ada tester allerede, og kan ikke startes på ny.",
                    "Start af ny test",
                    MessageBoxButtons.OK);
                return;
            }

            //            if (textBoxAV.Enabled == false)
            //            {
            //                System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
            //                return;
            //            }

            if (!settings.Drives.GetActiveDrives().Any())
            {
                MessageBox.Show("Der er ikke valgt nogle aktive drev ");
                return;
            }


            LogClear();
            status.Reset();
            LogRichTextBox.Focus(); //For at hindre enter i at udløse ny start test

            ID = new AViD(textBoxAV.Text);
            var mediaDirectories = AVMapping.GetMediaDirectories(ID, settings.Drives.GetActiveDrives());
            if (mediaDirectories.Count < 1)
            {
                MessageBox.Show("Afleveringen eksisterer ikke");
                return;
            }


            UC_DocumentViewer1.Update_ShellTree(mediaDirectories.First());
            UC_ContextDocumentViewer1.GetDocumentInfo(mediaDirectories.First(), true, false);

            textBoxAV.Enabled = false;
            browseAV.Enabled = false;
            tabPageSettings.Enabled = false;
            gbDeleteDBs.Enabled = false;

//            startButton.Text = "Start ny ADA";

            UC_DocumentViewer1.Update_ShellTree(mediaDirectories.First());
            UC_ContextDocumentViewer1.GetDocumentInfo(mediaDirectories.First(), true, false);


            StartTesting();
        }

        private void StartTesting()
        {
            if (running)
            {
                MessageBox.Show(
                    "Ada tester allerede, og kan ikke startes på ny.",
                    "Start af ny test",
                    MessageBoxButtons.OK);
                return;
            }

            running = true;

            startButton.Enabled = false;

            ClearDataContexts();

            testFactory?.Dispose();
            testFactory = null;
            logFactory?.Dispose();
            logFactory = null;
            avFactory?.Dispose();
            avFactory = null;

            UpdateWindowTitle();

            // disable UIs
            textBoxAV.Enabled = false;
            browseAV.Enabled = false;
            tabPageSettings.Enabled = false;
            gbDeleteDBs.Enabled = false;


            var path = new DirectoryInfo(Settings.Default.DBCreationFolder);
            testFactory = new AdaTestUowFactory(ID, "test", path);
            logFactory = new AdaLogUowFactory(ID, "log", path);
            avFactory = new AdaAvUowFactory(ID, "av", path);
            var version = new Version(1, 0);

            DialogResult dialogResult;

            if (logFactory.DataBaseExists())
                dialogResult = MessageBox.Show(
                    //                    "Der findes en tidligere kørsel af denne arkiveringsversion, ønsker du at fortsætte med at arbejde på denne?",
                    //                    "Eksisterende testsæt",
                    "Der findes en tidligere kørsel af denne arkiveringsversion, ønsker du at fortsætte med at arbejde på denne?" +
                    "\n\nHvis du vælger 'Nej' vil ADA slette data fra tidligere kørsel og køre en helt ny test.",
                    "Eksisterende testsæt",
                    MessageBoxButtons.YesNo);
            else
                dialogResult = DialogResult.No;

            if (dialogResult == DialogResult.No)
            {
                var t = Task.Run(
                    () =>
                    {
                        testFactory.DeleteDataBase();
                        testFactory.CreateDataBase();
                        using (var uow = (UnitOfWork) testFactory.GetUnitOfWork())
                        {
                            using (var cmd = uow.Session.Connection.CreateCommand())
                            {
                                cmd.CommandText = Resources.DbCreate;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        logFactory.DeleteDataBase();
                        logFactory.CreateDataBase();
                        var errorTexts =
                            new FileInfo(
                                Path.Combine(
                                    Path.GetDirectoryName(Application.ExecutablePath) ?? "",
                                    "errortexts.xml"));
                        AdaTestLog.LoadErrorTypesFromFile(errorTexts, logFactory);
                        avFactory.DeleteDataBase();
                        avFactory.CreateDataBase();
                    });
                t.Wait();
            }

            // testUow never to be disposed, as it is needed by ProgressManagementViewModel (for testResultsList)
            var testUow = (UnitOfWork) testFactory.GetUnitOfWork();

            var ucPLMDataContext = new ProgressManagementViewModel(testUow.Session.Connection);
            //                    ucPLMDataContext.RunTestRequested += UcPLMDataContext_RunTestRequested;
            ucPLM.DataContext = ucPLMDataContext;

            PrintRunSettingsToLogView();

            //Selve testen af AV'en starter
            ucTIV.DataContext = null;

            //settings.Serialize();
            new Task(
                () =>
                {
                    Controller.DoFullTest(ID, settings);
                    OnTestEnd();
                }
            ).Start();

            var pmVm = ucPLM.DataContext as ProgressManagementViewModel;
            if (pmVm != null)
                pmVm.TestLog = new AdaTestLog(logFactory, Guid.Empty, typeof(MainForm), 0);
        }

        private void textBoxAV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                LogRichTextBox.Focus();
                startButton.PerformClick();
                e.Handled = true;
            }
        }

        private void UcPLMDataContext_RunTestRequested(object sender, EventArgs e)
        {
            StartTesting();
        }

        private void UpdateWindowTitle()
        {
            var version = Application.ProductVersion;
            version = version.Remove(version.LastIndexOf('.'));

            Text = $"ADA v {version} ";
            Text += textBoxAV.Text ?? "";
            Text += running ? " (kører)" : "";
        }

        private void writeFileIndex(bool keepDocs)
        {
            Cursor = Cursors.WaitCursor;

            var avid = new AViD(textBoxAV.Text);

            //Create_1007_FileIndex c1007 = new Create_1007_FileIndex();
            var whiteList = new List<string>(settings.Drives.GetActiveDrives());

            if (whiteList.Count == 0)
            {
                MessageBox.Show("Der er ikke valgt nogle aktive drev i fanebladet Indstillinger!");
                Cursor = Cursors.Default;
                return;
            }

            var mapping = AVMapping.CreateMapping(avid, whiteList);


            if (mapping == null)
            {
                MessageBox.Show("Afleveringen eksisterer ikke");
                Cursor = Cursors.Default;
                return;
            }

            if (mapping.IsValid() == false)
            {
                MessageBox.Show("Der findes dubletter eller forkert navngivning af arkiveringsversionens AV medier!");
                Cursor = Cursors.Default;
                return;
            }


            DialogResult overWirteFile;
            if (keepDocs)
                overWirteFile = DialogResult.Yes;
            else
                overWirteFile = MessageBox.Show("Skal den eksisterende fileIndex.xml for " + textBoxAV.Text + " overskrives?",
                    "Skriv fileIndex.xml", MessageBoxButtons.YesNoCancel);

            if (overWirteFile == DialogResult.Yes)
            {
                //Form med tilhørende funktioner vises og sættes igang
                var CFI = new CreateFileIndexForm();
                CFI.Show();
                if (CFI.CreateFileIndex(avid.FullID, mapping.DistinctMappings, keepDocs))
                {
                    //Alt OK    
                    CFI.Close(); //Lukkes her for ikke at dække over Messagebox!
                    MessageBox.Show("fileIndex.xml for " + textBoxAV.Text + " opdateret.");
                }
                else
                {
                    //Fejl
                    CFI.Close(); //Lukkes her for ikke at dække over Messagebox!
                    MessageBox.Show("Der opstod en fejl i forbindelse med opdatering af fileIndex.xml for " +
                                    textBoxAV.Text); //Fejltype 20 fjernet REM
                }
            }
            else if (overWirteFile == DialogResult.No)
            {
                var folderSelected = folderBrowserDialog1.ShowDialog();
                if (folderSelected == DialogResult.OK)
                {
                    //Form med tilhørende funktioner vises og sættes igang
                    var CFI = new CreateFileIndexForm();
                    CFI.Show();
                    if (
                            CFI.CreateFileIndex(avid.FullID, mapping.DistinctMappings,
                                Path.Combine(folderBrowserDialog1.SelectedPath, "fileIndex.xml")))
                        //if (c1007.CreateFileIndex(avid.FullID, mapping.Mappings, Path.Combine(folderBrowserDialog1.SelectedPath, "fileIndex.xml")) == true)
                    {
                        //Alt OK 
                        CFI.Close(); //Lukkes her for ikke at dække over Messagebox!
                        MessageBox.Show("fileIndex.xml for " + textBoxAV.Text + " gemt som " +
                                        Path.Combine(folderBrowserDialog1.SelectedPath, "fileIndex.xml"));
                        //Fejltype 19 fjernet REM
                    }
                    else
                    {
                        //Fejl
                        CFI.Close(); //Lukkes her for ikke at dække over Messagebox!
                        MessageBox.Show("Der opstod en fejl i forbindelse med skabelse af fileIndex.xml for " +
                                        textBoxAV.Text); //Fejltype 18 fjernet REM
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void WriteLogOutput(object sender, AdaTestLog_EventHandlerArgs e)
        {
            if (e.Event.EntryTypeId == "4.D_12" || e.Event.EntryTypeId == "6.C_25")
            {
                int timesSeen;
                if (!RestrictedEvents.TryGetValue(e.Event.EntryTypeId, out timesSeen))
                    timesSeen = 0;

                if (timesSeen >= 10) return;
                RestrictedEvents[e.Event.EntryTypeId] = timesSeen + 1;
            }


            var text = e.Event.FormattedText;
//            text = text.Replace("¤¤", "\n\t\t");
//            text = text.Replace("¤", "\n\t");

            var color = Color.FromArgb(110, 110, 110);
            if (e.Event.Severity == 1) color = Color.FromArgb(160, 20, 20);
            SafeAddText(text, color);
        }

        #endregion
    }
}