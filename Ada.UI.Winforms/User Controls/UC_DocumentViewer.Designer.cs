using Ra.DocumentInvestigator.UsercontrolsWrappers;

namespace Ada.UI.Winforms.User_Controls
{
    using System.Windows.Forms;

    partial class UC_DocumentViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.AutoVisCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SkjulStifinderCheckBox = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonShowTifTags = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.rasterPictureBox1 = new RasterPictureBoxWrapper();
            this.shellTreeView1 = new Jam.Shell.ShellTreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shellTreeView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.progressBar1);
            this.flowLayoutPanel1.Controls.Add(this.AutoVisCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.SkjulStifinderCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.buttonPrint);
            this.flowLayoutPanel1.Controls.Add(this.buttonShowTifTags);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(678, 31);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(625, 3);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.progressBar1.Maximum = 50;
            this.progressBar1.Minimum = 1;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar1.Size = new System.Drawing.Size(50, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Value = 40;
            this.progressBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.progressBar1_MouseDown);
            this.progressBar1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.progressBar1_MouseMove);
            // 
            // AutoVisCheckBox
            // 
            this.AutoVisCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.AutoVisCheckBox.Location = new System.Drawing.Point(546, 3);
            this.AutoVisCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.AutoVisCheckBox.Name = "AutoVisCheckBox";
            this.AutoVisCheckBox.Size = new System.Drawing.Size(79, 23);
            this.AutoVisCheckBox.TabIndex = 13;
            this.AutoVisCheckBox.Text = "Auto Visning ";
            this.AutoVisCheckBox.UseVisualStyleBackColor = true;
            this.AutoVisCheckBox.CheckedChanged += new System.EventHandler(this.AutoVisCheckBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(530, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 28);
            this.panel1.TabIndex = 16;
            // 
            // SkjulStifinderCheckBox
            // 
            this.SkjulStifinderCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.SkjulStifinderCheckBox.Location = new System.Drawing.Point(440, 3);
            this.SkjulStifinderCheckBox.Name = "SkjulStifinderCheckBox";
            this.SkjulStifinderCheckBox.Size = new System.Drawing.Size(84, 23);
            this.SkjulStifinderCheckBox.TabIndex = 9;
            this.SkjulStifinderCheckBox.Text = "Skjul stifinder";
            this.SkjulStifinderCheckBox.UseVisualStyleBackColor = true;
            this.SkjulStifinderCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(424, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 28);
            this.panel2.TabIndex = 15;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Image = global::Ada.Properties.Resources.printer;
            this.buttonPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonPrint.Location = new System.Drawing.Point(333, 3);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(85, 23);
            this.buttonPrint.TabIndex = 4;
            this.buttonPrint.Text = "Print";
            this.buttonPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // buttonShowTifTags
            // 
            this.buttonShowTifTags.Location = new System.Drawing.Point(238, 3);
            this.buttonShowTifTags.Name = "buttonShowTifTags";
            this.buttonShowTifTags.Size = new System.Drawing.Size(89, 23);
            this.buttonShowTifTags.TabIndex = 5;
            this.buttonShowTifTags.Text = "Vis Tif Tags";
            this.buttonShowTifTags.UseVisualStyleBackColor = true;
            this.buttonShowTifTags.Click += new System.EventHandler(this.buttonShowTifTags_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(592, 34);
            this.listBox1.Name = "listBox1";
            this.listBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listBox1.Size = new System.Drawing.Size(83, 485);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            this.listBox1.Enter += new System.EventHandler(this.listBox1_Enter);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            this.listBox1.Leave += new System.EventHandler(this.listBox1_Leave);
            // 
            // rasterPictureBox1
            // 
            this.rasterPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rasterPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rasterPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.rasterPictureBox1.Name = "rasterPictureBox1";
            this.rasterPictureBox1.Size = new System.Drawing.Size(334, 485);
            this.rasterPictureBox1.SizeMode = "Fit";
            this.rasterPictureBox1.TabIndex = 5;
            this.rasterPictureBox1.TabStop = false;
            this.rasterPictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rasterPictureBox1_MouseUp);
            // 
            // shellTreeView1
            // 
            this.shellTreeView1.AllowDrag = false;
            this.shellTreeView1.AllowDrop = false;
            this.shellTreeView1.ChangeDelay = 500;
            this.shellTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellTreeView1.FileSystemOnly = true;
            this.shellTreeView1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.shellTreeView1.LabelEdit = true;
            this.shellTreeView1.Location = new System.Drawing.Point(0, 0);
            this.shellTreeView1.Name = "shellTreeView1";
            this.shellTreeView1.RootedAt = Jam.Shell.ShellFolder.Drives;
            this.shellTreeView1.RootedAtFileSystemFolder = "";
            this.shellTreeView1.SelectedPath = "";
            this.shellTreeView1.ShellContextMenu = false;
            this.shellTreeView1.ShowColorCompressed = System.Drawing.Color.Empty;
            this.shellTreeView1.ShowColorEncrypted = System.Drawing.Color.Empty;
            this.shellTreeView1.ShowContextMenu = false;
            this.shellTreeView1.ShowFiles = true;
            this.shellTreeView1.ShowLines = false;
            this.shellTreeView1.ShowRecycleBin = false;
            this.shellTreeView1.Size = new System.Drawing.Size(248, 485);
            this.shellTreeView1.Sorted = true;
            this.shellTreeView1.SpecialFolder = Jam.Shell.ShellFolder.Drives;
            this.shellTreeView1.TabIndex = 6;
            this.shellTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.shellTreeView1_AfterSelect_1);
            this.shellTreeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.shellTreeView1_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(2, 34);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rasterPictureBox1);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.shellTreeView1);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(586, 485);
            this.splitContainer1.SplitterDistance = 334;
            this.splitContainer1.TabIndex = 7;
            // 
            // UC_DocumentViewer
            // 
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "UC_DocumentViewer";
            this.Size = new System.Drawing.Size(678, 529);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shellTreeView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonPrint;
        private Button buttonShowTifTags;
        private ListBox listBox1;
        private RasterPictureBoxWrapper rasterPictureBox1;
        private Jam.Shell.ShellTreeView shellTreeView1;
        private CheckBox SkjulStifinderCheckBox;
        private ProgressBar progressBar1;
        private CheckBox AutoVisCheckBox;
        private Panel panel2;
        private Panel panel1;
        private SplitContainer splitContainer1;

        
    }
}
