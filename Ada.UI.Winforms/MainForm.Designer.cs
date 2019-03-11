using System.Windows.Controls;

namespace Ada.UI.Winforms
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;
    
    using Ada.UI.Winforms.User_Controls;
    using Ada.UI.Wpf.ProgressManagement.ViewModel;
    using Ada.UI.Wpf.WorkspaceCleanUp.ViewModel;
    using Wpf.ProgressManagement.Views.View;
    using Wpf.TableIndexViewer.Views;
    using Wpf.WorkspaceCleanUp.Views.View;
    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    partial class MainForm
    { 
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBox_Select_Arkiveringsversion = new System.Windows.Forms.GroupBox();
            this.startButton = new System.Windows.Forms.Button();
            this.browseAV = new System.Windows.Forms.Button();
            this.textBoxAV = new System.Windows.Forms.TextBox();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.LOGgroupBox = new System.Windows.Forms.GroupBox();
            this.LogRichTextBox = new System.Windows.Forms.RichTextBox();
            this.tabControlLog = new System.Windows.Forms.TabControl();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.tabPageProgress = new System.Windows.Forms.TabPage();
            this.buttonPrintLog = new System.Windows.Forms.Button();
            this.butInspec1 = new System.Windows.Forms.Button();
            this.buttonSaveLogAs = new System.Windows.Forms.Button();
            this.groupBoxForTabControl = new System.Windows.Forms.GroupBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.tabPageContextDocument = new System.Windows.Forms.TabPage();
            this.tabPageDokument = new System.Windows.Forms.TabPage();
            this.tabPageTableViewWpf = new System.Windows.Forms.TabPage();
            this.tabPageTools = new System.Windows.Forms.TabPage();
            this.groupBoxArkiveringsversionTools = new System.Windows.Forms.GroupBox();
            this.buttonCreateFileIndex = new System.Windows.Forms.Button();
            this.buttonUpdateFileIndex = new System.Windows.Forms.Button();
//            this.buttonRewriteFileIndex = new System.Windows.Forms.Button();
//            this.buttonRewriteTableXsds = new System.Windows.Forms.Button();
            this.groupBoxOpenIndexFiles = new System.Windows.Forms.GroupBox();
            this.gbDeleteDBs = new System.Windows.Forms.GroupBox();
            this.buttonOpenArchiveIndex = new System.Windows.Forms.Button();
            this.buttonOpenContextDocumentationIndex = new System.Windows.Forms.Button();
            this.buttonOpenFileIndex = new System.Windows.Forms.Button();
            this.buttonOpenTableIndex = new System.Windows.Forms.Button();
            this.buttonOpenDocIndex = new System.Windows.Forms.Button();
            this.tabPageHelp = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
             this.UC_ContextDocumentViewer1 = new UC_ContextDocumentViewer();
            this.UC_DocumentViewer1 = new UC_DocumentViewer();
            this.UC_WebBrowser1 = new UC_WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit(); 
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBox_Select_Arkiveringsversion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.LOGgroupBox.SuspendLayout();
            this.tabControlLog.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            
            this.tabPageProgress.SuspendLayout();
            this.groupBoxForTabControl.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageContextDocument.SuspendLayout();
            this.tabPageDokument.SuspendLayout();
            this.tabPageTableViewWpf.SuspendLayout();
            this.tabPageTools.SuspendLayout();
            this.groupBoxArkiveringsversionTools.SuspendLayout();
            this.groupBoxOpenIndexFiles.SuspendLayout();
            this.tabPageHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
//            this.splitContainerMain.Panel1.Controls.Add(this.tabControlLog);
            this.splitContainerMain.Panel1MinSize = 0;
//            this.splitContainerMain.Panel1Collapsed = true;
            this.splitContainerMain.Panel1.Controls.Add(this.groupBox_Select_Arkiveringsversion);
            this.splitContainerMain.Panel1MinSize = 220;
            
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerMain.Size = new System.Drawing.Size(1316, 730);
            this.splitContainerMain.SplitterDistance = 300;
            this.splitContainerMain.SplitterWidth = 8;
            this.splitContainerMain.TabIndex = 1;
            // 
            // groupBox_Select_Arkiveringsversion
            // 
            this.groupBox_Select_Arkiveringsversion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Select_Arkiveringsversion.Controls.Add(this.startButton);
            this.groupBox_Select_Arkiveringsversion.Controls.Add(this.browseAV);
            this.groupBox_Select_Arkiveringsversion.Controls.Add(this.textBoxAV);
            this.groupBox_Select_Arkiveringsversion.Location = new System.Drawing.Point(7, 2);
            this.groupBox_Select_Arkiveringsversion.Name = "groupBox_Select_Arkiveringsversion";
            this.groupBox_Select_Arkiveringsversion.Size = new System.Drawing.Size(293, 90);
            this.groupBox_Select_Arkiveringsversion.TabIndex = 9;
            this.groupBox_Select_Arkiveringsversion.TabStop = false;
            this.groupBox_Select_Arkiveringsversion.Text = "Arkiveringsversion";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(15, 49);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(95, 23);
            this.startButton.TabIndex = 9;
            this.startButton.Text = "Start test";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // browseAV
            // 
            this.browseAV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseAV.Location = new System.Drawing.Point(248, 23);
            this.browseAV.Name = "browseAV";
            this.browseAV.Size = new System.Drawing.Size(27, 21);
            this.browseAV.TabIndex = 11;
            this.browseAV.Text = "...";
            this.browseAV.UseVisualStyleBackColor = true;
            this.browseAV.Click += new System.EventHandler(this.BrowseAV_Click);
            // 
            // textBoxAV
            // 
            this.textBoxAV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAV.Location = new System.Drawing.Point(15, 23);
            this.textBoxAV.Name = "textBoxAV";
            this.textBoxAV.Size = new System.Drawing.Size(227, 20);
            this.textBoxAV.TabIndex = 10;
            this.textBoxAV.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAV_KeyDown);
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //  
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.tabControlLog);
            this.splitContainerRight.Panel1MinSize = 86;
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.groupBoxForTabControl);
            this.splitContainerRight.Panel2MinSize = 46;
            this.splitContainerRight.Size = new System.Drawing.Size(1008, 730);
            this.splitContainerRight.SplitterDistance = 286;
            this.splitContainerRight.SplitterWidth = 16;
            this.splitContainerRight.TabIndex = 4;
            this.splitContainerRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerRight_SplitterMoved);
            // 
            // LOGgroupBox
            // 
            this.LOGgroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LOGgroupBox.Controls.Add(this.LogRichTextBox);
            this.LOGgroupBox.Controls.Add(this.buttonPrintLog);
            this.LOGgroupBox.Controls.Add(this.butInspec1);
            this.LOGgroupBox.Controls.Add(this.buttonSaveLogAs);
            this.LOGgroupBox.Location = new System.Drawing.Point(6, 2);
            this.LOGgroupBox.Name = "LOGgroupBox";
            this.LOGgroupBox.Size = new System.Drawing.Size(959, 250);// 256 );
            this.LOGgroupBox.TabIndex = 1;
            this.LOGgroupBox.TabStop = false;
            this.LOGgroupBox.Text = "LOG";
            // 
            // tabControlLog
            // 
            this.tabControlLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlLog.Controls.Add(this.tabPageLog);
            this.tabControlLog.Controls.Add(this.tabPageProgress);
//            this.tabControlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLog.Location = new System.Drawing.Point(6, 2);
            this.tabControlLog.Name = "tabControl1";
            this.tabControlLog.SelectedIndex = 0;
            this.tabControlLog.Size = new System.Drawing.Size(995, 281);
            this.tabControlLog.TabIndex = 0;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPageLog.Controls.Add(this.LOGgroupBox);
//            this.tabPageLog.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageSettings";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(981, 271);
            this.tabPageLog.TabIndex = 2;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            //tabPageLog = null;
            // 
            // LogRichTextBox
            // 
            this.LogRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogRichTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogRichTextBox.Location = new System.Drawing.Point(7, 71);
            this.LogRichTextBox.Name = "LogRichTextBox";
//            this.LogRichTextBox.Dock = DockStyle.Fill;
            this.LogRichTextBox.Size = new System.Drawing.Size(935, 170);
            this.LogRichTextBox.TabIndex = 1;
            this.LogRichTextBox.Text = "";
            this.LogRichTextBox.WordWrap = false;
            this.LogRichTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LogRichTextBox_MouseDoubleClick);

            ucPLM = new ProgressManagementView();
            ucPLM.InitializeComponent();
            ucPLM.HorizontalAlignment = HorizontalAlignment.Stretch;
            ucPLM.VerticalAlignment = VerticalAlignment.Stretch;
            var ucPLMDataContext = new ProgressManagementViewModel();
            ucPLMDataContext.RunTestRequested += UcPLMDataContext_RunTestRequested;
            ucPLM.DataContext = ucPLMDataContext;

            ElementHost elementHostPLM = new ElementHost();
            elementHostPLM.Child = ucPLM;
            elementHostPLM.AutoSize = true;
            elementHostPLM.Dock = DockStyle.Fill;


            // 
            // tabPageProgress
            // 
            this.tabPageProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
            | System.Windows.Forms.AnchorStyles.Left))));
            this.tabPageProgress.Controls.Add(elementHostPLM);
//            this.tabPageProgress.Location = new System.Drawing.Point(4, 22);
            this.tabPageProgress.Name = "tabPageSettings";
            this.tabPageProgress.Padding = new System.Windows.Forms.Padding(3);
                        this.tabPageProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            //            this.tabPageProgress.Size = new System.Drawing.Size(981, 344);
            //            this.tabPageProgress.TabIndex = 2;
            this.tabPageProgress.Text = "Progress Log";
//            this.tabPageProgress.UseVisualStyleBackColor = true;






            // 
            // buttonPrintLog
            // 
            this.buttonPrintLog.Location = new System.Drawing.Point(103, 42);
            this.buttonPrintLog.Name = "buttonPrintLog";
            this.buttonPrintLog.Size = new System.Drawing.Size(90, 23);
            this.buttonPrintLog.TabIndex = 1;
            this.buttonPrintLog.Text = "Udskriv log";
            this.buttonPrintLog.UseVisualStyleBackColor = true;
            this.buttonPrintLog.Click += new System.EventHandler(this.buttonPrintLog_Click);
            // 
            // butInspec1
            // 
            this.butInspec1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butInspec1.Image = ((System.Drawing.Image)(resources.GetObject("butInspec1.Image")));
            this.butInspec1.Location = new System.Drawing.Point(926, 11);
            this.butInspec1.Name = "butInspec1";
            this.butInspec1.Size = new System.Drawing.Size(22, 22);
            this.butInspec1.TabIndex = 3;
            this.butInspec1.UseVisualStyleBackColor = true;
            this.butInspec1.Click += new System.EventHandler(this.butInspec1_Click);
            // 
            // buttonSaveLogAs
            // 
            this.buttonSaveLogAs.Location = new System.Drawing.Point(7, 42);
            this.buttonSaveLogAs.Name = "buttonSaveLogAs";
            this.buttonSaveLogAs.Size = new System.Drawing.Size(90, 23);
            this.buttonSaveLogAs.TabIndex = 0;
            this.buttonSaveLogAs.Text = "Gem log som ...";
            this.buttonSaveLogAs.UseVisualStyleBackColor = true;
            this.buttonSaveLogAs.Click += new System.EventHandler(this.buttonSaveLogAs_Click);
            // 
            // groupBoxForTabControl
            // 
            this.groupBoxForTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxForTabControl.Controls.Add(this.tabControl);
            this.groupBoxForTabControl.Location = new System.Drawing.Point(0, 3);
            this.groupBoxForTabControl.Name = "groupBoxForTabControl";
            this.groupBoxForTabControl.Size = new System.Drawing.Size(1001, 389);
            this.groupBoxForTabControl.TabIndex = 1;
            this.groupBoxForTabControl.TabStop = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Controls.Add(this.tabPageContextDocument);
            this.tabControl.Controls.Add(this.tabPageDokument);
            this.tabControl.Controls.Add(this.tabPageTableViewWpf);
            this.tabControl.Controls.Add(this.tabPageTools);
            this.tabControl.Controls.Add(this.tabPageHelp);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(3, 16);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(989, 370);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.AutoScroll = true;
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(981, 344);
            this.tabPageSettings.TabIndex = 2;
            this.tabPageSettings.Text = "Indstillinger";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tabPageContextDocument
            // 
            this.tabPageContextDocument.Controls.Add(this.UC_ContextDocumentViewer1);
            this.tabPageContextDocument.Location = new System.Drawing.Point(4, 22);
            this.tabPageContextDocument.Name = "tabPageContextDocument";
            this.tabPageContextDocument.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContextDocument.Size = new System.Drawing.Size(981, 344);
            this.tabPageContextDocument.TabIndex = 7;
            this.tabPageContextDocument.Text = "ContextDocumentation";
            this.tabPageContextDocument.UseVisualStyleBackColor = true;
            // 
            // tabPageDokument
            // 
            this.tabPageDokument.Controls.Add(this.UC_DocumentViewer1);
            this.tabPageDokument.Location = new System.Drawing.Point(4, 22);
            this.tabPageDokument.Name = "tabPageDokument";
            this.tabPageDokument.Size = new System.Drawing.Size(981, 344);
            this.tabPageDokument.TabIndex = 0;
            this.tabPageDokument.Text = "Dokument Viewer";
            
            
            // 
            // tabPageTableViewWpf
            // 

            

            ucTIV = new TableIndexViewer();
            ucTIV.InitializeComponent();
            ucTIV.HorizontalAlignment = HorizontalAlignment.Stretch;
            ucTIV.VerticalAlignment = VerticalAlignment.Stretch;
            uucTIV = new TableIndexViewer();
            uucTIV.InitializeComponent();
            uucTIV.HorizontalAlignment = HorizontalAlignment.Stretch;
            uucTIV.VerticalAlignment = VerticalAlignment.Stretch;
            TableViewerTabPopup = new ElementHost();
            TableViewerTabPopup.Child = uucTIV;
            TableViewerTabPopup.AutoSize = true;
            TableViewerTabPopup.Dock = DockStyle.Fill;


            //            ucPLM. .CommandBindings. AddRange(ucTIV.CommandBindings);


            TableViewerTab = new ElementHost();
            TableViewerTab.Child = ucTIV;
            TableViewerTab.AutoSize = true;
            TableViewerTab.Dock = DockStyle.Fill;
            this.tabPageTableViewWpf.Controls.Add(TableViewerTab);
            this.tabPageTableViewWpf.Location = new System.Drawing.Point(4, 22);
            this.tabPageTableViewWpf.Name = "tabPageTableViewWpf";
            this.tabPageTableViewWpf.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTableViewWpf.Size = new System.Drawing.Size(981, 344);
            this.tabPageTableViewWpf.TabIndex = 8;
            this.tabPageTableViewWpf.Text = "Tabel Viewer";
            this.tabPageTableViewWpf.UseVisualStyleBackColor = true;

            

            // 
            // tabPageTools
            // 
            this.tabPageTools.Controls.Add(this.groupBoxArkiveringsversionTools);
            this.tabPageTools.Controls.Add(this.groupBoxOpenIndexFiles);
            this.tabPageTools.Controls.Add(this.gbDeleteDBs);
            this.tabPageTools.Location = new System.Drawing.Point(4, 22);
            this.tabPageTools.Name = "tabPageTools";
            this.tabPageTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTools.Size = new System.Drawing.Size(981, 344);
            this.tabPageTools.TabIndex = 3;
            this.tabPageTools.Text = "Værktøjer";
            this.tabPageTools.UseVisualStyleBackColor = true;
            // 
            // gbDeleteDBs
            // 


            ucWCU = new WorkspaceCleanUpView();
            ucWCU.InitializeComponent();
            ucWCU.HorizontalAlignment = HorizontalAlignment.Stretch;
            ucWCU.VerticalAlignment = VerticalAlignment.Stretch;

            ElementHost elementHost2 = new ElementHost();
            elementHost2.Child = ucWCU;
            elementHost2.AutoSize = true;
            elementHost2.Dock = DockStyle.Fill;
            this.gbDeleteDBs.Controls.Add(elementHost2);
            this.gbDeleteDBs.Location = new System.Drawing.Point(255 + 186 + (255 - (215 + 17)), 15);
            this.gbDeleteDBs.Name = "gbDeleteDBs";
            this.gbDeleteDBs.Size = new System.Drawing.Size(186, 187);
            this.gbDeleteDBs.TabIndex = 7;
            this.gbDeleteDBs.TabStop = false;
            this.gbDeleteDBs.Text = "DB placering";   // 


            ucCUC = new CleanUpCurrentView();
            ucCUC.DataContext = new CleanUpCurrentViewModel();
            ucCUC.InitializeComponent();
                        ucWCU.HorizontalAlignment = HorizontalAlignment.Stretch;
            ucCUC.VerticalAlignment = VerticalAlignment.Stretch;

            ElementHost elementHostucCUC = new ElementHost();
            elementHostucCUC.Location = new System.Drawing.Point(33, 89);
//            elementHostucCUC.Location = new System.Drawing.Point(33, 89 + (89-60));
            elementHostucCUC.Size = new System.Drawing.Size(140, 120);
            elementHostucCUC.Child = ucCUC;
            elementHostucCUC.Dock = DockStyle.Top;

            // 
            // groupBoxArkiveringsversionTools
            // 
            this.groupBoxArkiveringsversionTools.Controls.Add(this.buttonCreateFileIndex);
            this.groupBoxArkiveringsversionTools.Controls.Add(this.buttonUpdateFileIndex);
//            this.groupBoxArkiveringsversionTools.Controls.Add(this.buttonRewriteFileIndex);
//            this.groupBoxArkiveringsversionTools.Controls.Add(this.buttonRewriteTableXsds);
            this.groupBoxArkiveringsversionTools.Controls.Add(elementHostucCUC);
            this.groupBoxArkiveringsversionTools.Location = new System.Drawing.Point(255, 15);
            this.groupBoxArkiveringsversionTools.Name = "groupBoxArkiveringsversionTools";
            this.groupBoxArkiveringsversionTools.Size = new System.Drawing.Size(186, 187);
            this.groupBoxArkiveringsversionTools.TabIndex = 7;
            this.groupBoxArkiveringsversionTools.TabStop = false;
            this.groupBoxArkiveringsversionTools.Text = "Arkiveringsversion";
            // 
            // buttonCreateFileIndex
            // 
            this.buttonCreateFileIndex.Location = new System.Drawing.Point(33, 33);
            this.buttonCreateFileIndex.Name = "buttonCreateFileIndex";
            this.buttonCreateFileIndex.Size = new System.Drawing.Size(140, 23);
            this.buttonCreateFileIndex.TabIndex = 0;
            this.buttonCreateFileIndex.Text = "Skab ny fileIndex.xml";
            this.toolTip1.SetToolTip(this.buttonCreateFileIndex, "Skaber nyt FileIndex.xml med opdaterede MD5 værdier");
            this.buttonCreateFileIndex.UseVisualStyleBackColor = true;
            this.buttonCreateFileIndex.Click += new System.EventHandler(this.buttonCreateFileIndex_Click);
            // 
            // buttonCreateFileIndex
            // 
            this.buttonUpdateFileIndex.Location = new System.Drawing.Point(33, 60);
            this.buttonUpdateFileIndex.Name = "buttonUpdateFileIndex";
            this.buttonUpdateFileIndex.Size = new System.Drawing.Size(140, 23);
            this.buttonUpdateFileIndex.TabIndex = 0;
            this.buttonUpdateFileIndex.Text = "Opdater fileIndex.xml (minus dokumenter)";
            this.toolTip1.SetToolTip(this.buttonUpdateFileIndex, "Opdatere FileIndex.xml med opdaterede MD5 værdier, minus dokumenter");
            this.buttonUpdateFileIndex.UseVisualStyleBackColor = true;
            this.buttonUpdateFileIndex.Click += new System.EventHandler(this.buttonUpdateFileIndex_Click);
//            // 
//            // buttonRewriteFileIndex
//            // 
//            this.buttonRewriteFileIndex.Location = new System.Drawing.Point(33, 60);
//            this.buttonRewriteFileIndex.Name = "buttonRewriteFileIndex";
//            this.buttonRewriteFileIndex.Size = new System.Drawing.Size(140, 23);
//            this.buttonRewriteFileIndex.TabIndex = 0;
//            this.buttonRewriteFileIndex.Text = "Genskriv fileIndex.xml";
////            this.toolTip1.SetToolTip(this.buttonRewriteFileIndex, "Skaber nyt FileIndex.xml med opdaterede MD5 værdier");
//            this.buttonRewriteFileIndex.UseVisualStyleBackColor = true;
//            this.buttonRewriteFileIndex.Click += new System.EventHandler(this.buttonRewriteFileIndex_Click);
//            this.buttonRewriteFileIndex.Enabled = false;
//            // 
//            // buttonRewriteTableXsds
//            // 
//            this.buttonRewriteTableXsds.Location = new System.Drawing.Point(33, 89);
//            this.buttonRewriteTableXsds.Name = "buttonRewriteTableXsds";
//            this.buttonRewriteTableXsds.Size = new System.Drawing.Size(140, 23);
//            this.buttonRewriteTableXsds.TabIndex = 0;
//            this.buttonRewriteTableXsds.Text = "Genskriv table-xsd'ere";
////            this.toolTip1.SetToolTip(this.buttonRewriteTableXsds, "Skaber nyt FileIndex.xml med opdaterede MD5 værdier");
//            this.buttonRewriteTableXsds.UseVisualStyleBackColor = true;
//            this.buttonRewriteTableXsds.Click += new System.EventHandler(this.buttonRewriteTableXsds_Click);
//            this.buttonRewriteTableXsds.Enabled = false;
            // 
            // groupBoxOpenIndexFiles
            // 
            this.groupBoxOpenIndexFiles.Controls.Add(this.buttonOpenArchiveIndex);
            this.groupBoxOpenIndexFiles.Controls.Add(this.buttonOpenContextDocumentationIndex);
            this.groupBoxOpenIndexFiles.Controls.Add(this.buttonOpenFileIndex);
            this.groupBoxOpenIndexFiles.Controls.Add(this.buttonOpenTableIndex);
            this.groupBoxOpenIndexFiles.Controls.Add(this.buttonOpenDocIndex);
            this.groupBoxOpenIndexFiles.Location = new System.Drawing.Point(17, 15);
            this.groupBoxOpenIndexFiles.Name = "groupBoxOpenIndexFiles";
            this.groupBoxOpenIndexFiles.Size = new System.Drawing.Size(215, 187);
            this.groupBoxOpenIndexFiles.TabIndex = 6;
            this.groupBoxOpenIndexFiles.TabStop = false;
            this.groupBoxOpenIndexFiles.Text = "Åben indexfil";
            // 
            // buttonOpenArchiveIndex
            // 
            this.buttonOpenArchiveIndex.Location = new System.Drawing.Point(20, 33);
            this.buttonOpenArchiveIndex.Name = "buttonOpenArchiveIndex";
            this.buttonOpenArchiveIndex.Size = new System.Drawing.Size(170, 23);
            this.buttonOpenArchiveIndex.TabIndex = 10;
            this.buttonOpenArchiveIndex.Text = "archiveIndex.xml";
            this.buttonOpenArchiveIndex.UseVisualStyleBackColor = true;
            this.buttonOpenArchiveIndex.Click += new System.EventHandler(this.buttonOpenArchiveIndex_Click);
            // 
            // buttonOpenContextDocumentationIndex
            // 
            this.buttonOpenContextDocumentationIndex.Location = new System.Drawing.Point(20, 147);
            this.buttonOpenContextDocumentationIndex.Name = "buttonOpenContextDocumentationIndex";
            this.buttonOpenContextDocumentationIndex.Size = new System.Drawing.Size(170, 23);
            this.buttonOpenContextDocumentationIndex.TabIndex = 9;
            this.buttonOpenContextDocumentationIndex.Text = "contextDocumentationIndex.xml";
            this.buttonOpenContextDocumentationIndex.UseVisualStyleBackColor = true;
            this.buttonOpenContextDocumentationIndex.Click += new System.EventHandler(this.buttonOpenContextDocumentationIndex_Click);
            // 
            // buttonOpenFileIndex
            // 
            this.buttonOpenFileIndex.Location = new System.Drawing.Point(20, 118);
            this.buttonOpenFileIndex.Name = "buttonOpenFileIndex";
            this.buttonOpenFileIndex.Size = new System.Drawing.Size(170, 23);
            this.buttonOpenFileIndex.TabIndex = 8;
            this.buttonOpenFileIndex.Text = "fileIndex.xml";
            this.buttonOpenFileIndex.UseVisualStyleBackColor = true;
            this.buttonOpenFileIndex.Click += new System.EventHandler(this.buttonOpenFileIndex_Click);
            // 
            // buttonOpenTableIndex
            // 
            this.buttonOpenTableIndex.Location = new System.Drawing.Point(20, 89);
            this.buttonOpenTableIndex.Name = "buttonOpenTableIndex";
            this.buttonOpenTableIndex.Size = new System.Drawing.Size(170, 23);
            this.buttonOpenTableIndex.TabIndex = 7;
            this.buttonOpenTableIndex.Text = "tableIndex.xml";
            this.buttonOpenTableIndex.UseVisualStyleBackColor = true;
            this.buttonOpenTableIndex.Click += new System.EventHandler(this.buttonOpenTableIndex_Click);
            // 
            // buttonOpenDocIndex
            // 
            this.buttonOpenDocIndex.Location = new System.Drawing.Point(20, 60);
            this.buttonOpenDocIndex.Name = "buttonOpenDocIndex";
            this.buttonOpenDocIndex.Size = new System.Drawing.Size(170, 23);
            this.buttonOpenDocIndex.TabIndex = 6;
            this.buttonOpenDocIndex.Text = "docIndex.xml";
            this.buttonOpenDocIndex.UseVisualStyleBackColor = true;
            this.buttonOpenDocIndex.Click += new System.EventHandler(this.buttonOpenDocIndex_Click);
            // 
            // tabPageHelp
            // 
            this.tabPageHelp.Controls.Add(this.UC_WebBrowser1);
            this.tabPageHelp.Location = new System.Drawing.Point(4, 22);
            this.tabPageHelp.Name = "tabPageHelp";
            this.tabPageHelp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHelp.Size = new System.Drawing.Size(981, 344);
            this.tabPageHelp.TabIndex = 5;
            this.tabPageHelp.Text = "Hjælp";
            this.tabPageHelp.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 708);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1316, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(300, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(967, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.rtf";
            this.saveFileDialog1.Filter = "RTF dokument|*.rtf";
            this.saveFileDialog1.Title = "Gem log som ...";
            // 
            // UC_ContextDocumentViewer1
            // 
            this.UC_ContextDocumentViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_ContextDocumentViewer1.Location = new System.Drawing.Point(3, 3);
            this.UC_ContextDocumentViewer1.Name = "UC_ContextDocumentViewer1";
            this.UC_ContextDocumentViewer1.Size = new System.Drawing.Size(975, 338);
            this.UC_ContextDocumentViewer1.TabIndex = 0;
            // 
            // UC_DocumentViewer1
            // 
            this.UC_DocumentViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_DocumentViewer1.Location = new System.Drawing.Point(0, 0);
            this.UC_DocumentViewer1.Name = "UC_DocumentViewer1";
            this.UC_DocumentViewer1.Size = new System.Drawing.Size(981, 344);
            this.UC_DocumentViewer1.TabIndex = 0;
            // 
            // UC_WebBrowser1
            // 
            this.UC_WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_WebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.UC_WebBrowser1.Name = "UC_WebBrowser1";
            this.UC_WebBrowser1.Size = new System.Drawing.Size(975, 338);
            this.UC_WebBrowser1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1316, 730);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainerMain);
            this.Icon =
                ((System.Drawing.Icon)
                 (new System.Drawing.Icon(
                     this.GetType().Assembly.GetManifestResourceStream("Ada.UI.Winforms.Krone_blaa.ico"))));
                
                
                
                //Resources resources.GetObject("")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADA ...";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += MainForm_FormClosing;
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.groupBox_Select_Arkiveringsversion.ResumeLayout(false);
            this.groupBox_Select_Arkiveringsversion.PerformLayout();
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.LOGgroupBox.ResumeLayout(false);
            this.tabControlLog.ResumeLayout(false);
            if(tabPageLog != null)
            this.tabPageLog.ResumeLayout(false);
            this.tabPageProgress.ResumeLayout(false);
            this.groupBoxForTabControl.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageContextDocument.ResumeLayout(false);
            this.tabPageDokument.ResumeLayout(false);
            this.tabPageTableViewWpf.ResumeLayout(false);
            this.tabPageTools.ResumeLayout(false);
            this.groupBoxArkiveringsversionTools.ResumeLayout(false);
            this.groupBoxOpenIndexFiles.ResumeLayout(false);
            this.tabPageHelp.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void ToggleFloatTableViewer()
        {
            if (TableViewFormFloating)
            {
                TableViewFormFloating = false;
                if (PopupWindow.hidden)
                {
                    ToggleFloatTableViewer();
                }
                else
                {
                    TableViewForm.Close();
                }
            }
            else
            {
                TableViewForm = new PopupWindow(this);
                this.tabPageTableViewWpf.Controls.Add(TableViewerTabPopup);
                this.tabPageTableViewWpf.Location = new System.Drawing.Point(4, 22);
                this.tabPageTableViewWpf.Name = "tabPageTableViewWpf";
                this.tabPageTableViewWpf.Padding = new System.Windows.Forms.Padding(3);
                this.tabPageTableViewWpf.Size = new System.Drawing.Size(981, 344);
                this.tabPageTableViewWpf.TabIndex = 8;
                this.tabPageTableViewWpf.Text = "Tabel Viewer";
                this.tabPageTableViewWpf.UseVisualStyleBackColor = true;

               
                TableViewForm.Name = "tabPageTableViewWpf";
                TableViewForm.Text = "Tabelviser";
                //Controls.Remove(TableViewerTab);
                TableViewForm.Controls.Add(TableViewerTabPopup);
                TableViewForm.Show();
                TableViewFormFloating = true;
            }
            
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button buttonPrintLog;
        private System.Windows.Forms.Button buttonSaveLogAs;
        private System.Windows.Forms.RichTextBox LogRichTextBox;
        public System.Windows.Forms.TabControl tabControlLog;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.TabPage tabPageProgress;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button butInspec1;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.TabPage tabPageDokument;
        private System.Windows.Forms.GroupBox groupBox_Select_Arkiveringsversion;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button browseAV;

        public System.Windows.Forms.TextBox textBoxAV;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox LOGgroupBox;
        private System.Windows.Forms.GroupBox groupBoxForTabControl;
        public System.Windows.Forms.TabControl tabControl;
        private UC_DocumentViewer UC_DocumentViewer1;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TabPage tabPageTools;
        private System.Windows.Forms.TabPage tabPageHelp;
        private UC_WebBrowser UC_WebBrowser1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBoxArkiveringsversionTools;
        private System.Windows.Forms.Button buttonCreateFileIndex;
        private System.Windows.Forms.Button buttonUpdateFileIndex;
//        private System.Windows.Forms.Button buttonRewriteFileIndex;
//        private System.Windows.Forms.Button buttonRewriteTableXsds;
        private System.Windows.Forms.GroupBox groupBoxOpenIndexFiles;
        private System.Windows.Forms.GroupBox gbDeleteDBs;
        private System.Windows.Forms.Button buttonOpenDocIndex;
        private System.Windows.Forms.Button buttonOpenContextDocumentationIndex;
        private System.Windows.Forms.Button buttonOpenFileIndex;
        private System.Windows.Forms.Button buttonOpenTableIndex;
        private System.Windows.Forms.Button buttonOpenArchiveIndex;
        private System.Windows.Forms.TabPage tabPageContextDocument;
        private UC_ContextDocumentViewer UC_ContextDocumentViewer1;
        private System.Windows.Forms.TabPage tabPageTableViewWpf;
        private TableIndexViewer ucTIV;
        private WorkspaceCleanUpView ucWCU;
        private CleanUpCurrentView ucCUC;
        private ProgressManagementView ucPLM;
    }
}


public class PopupWindow
{
public string EnteredText
{
get
{
    return (new TextBox().Text);
}
}
}
