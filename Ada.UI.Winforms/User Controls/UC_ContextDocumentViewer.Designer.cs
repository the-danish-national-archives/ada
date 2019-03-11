using Ra.DocumentInvestigator.UsercontrolsWrappers;

namespace Ada.UI.Winforms.User_Controls
{
    partial class UC_ContextDocumentViewer
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonShowTifTags = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.rasterPictureBox1 = new RasterPictureBoxWrapper();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.docListBox = new System.Windows.Forms.ListBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.buttonPrint);
            this.flowLayoutPanel1.Controls.Add(this.buttonShowTifTags);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(801, 31);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(748, 3);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.progressBar1.Maximum = 50;
            this.progressBar1.Minimum = 1;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar1.Size = new System.Drawing.Size(50, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 17;
            this.progressBar1.Value = 40;
            this.progressBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.progressBar1_MouseDown);
            this.progressBar1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.progressBar1_MouseMove);
            // 
            // AutoVisCheckBox
            // 
            this.AutoVisCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.AutoVisCheckBox.Location = new System.Drawing.Point(669, 3);
            this.AutoVisCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.AutoVisCheckBox.Name = "AutoVisCheckBox";
            this.AutoVisCheckBox.Size = new System.Drawing.Size(79, 23);
            this.AutoVisCheckBox.TabIndex = 18;
            this.AutoVisCheckBox.Text = "Auto Visning ";
            this.AutoVisCheckBox.UseVisualStyleBackColor = true;
            this.AutoVisCheckBox.CheckedChanged += new System.EventHandler(this.AutoVisCheckBox_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(653, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 28);
            this.panel2.TabIndex = 16;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Image = global::Ada.Properties.Resources.printer;
            this.buttonPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonPrint.Location = new System.Drawing.Point(562, 3);
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
            this.buttonShowTifTags.Location = new System.Drawing.Point(467, 3);
            this.buttonShowTifTags.Name = "buttonShowTifTags";
            this.buttonShowTifTags.Size = new System.Drawing.Size(89, 23);
            this.buttonShowTifTags.TabIndex = 5;
            this.buttonShowTifTags.Text = "Vis Tif Tags";
            this.buttonShowTifTags.UseVisualStyleBackColor = true;
            this.buttonShowTifTags.Click += new System.EventHandler(this.buttonShowTifTags_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(386, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Åben ...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rasterPictureBox1
            // 
            this.rasterPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rasterPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rasterPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.rasterPictureBox1.Name = "rasterPictureBox1";
            this.rasterPictureBox1.Size = new System.Drawing.Size(388, 498);
            this.rasterPictureBox1.SizeMode = "Fit";
            this.rasterPictureBox1.TabIndex = 9;
            this.rasterPictureBox1.TabStop = false;
            this.rasterPictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rasterPictureBox1_MouseUp);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(715, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listBox1.Size = new System.Drawing.Size(83, 498);
            this.listBox1.TabIndex = 8;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            this.listBox1.Enter += new System.EventHandler(this.listBox1_Enter);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            this.listBox1.Leave += new System.EventHandler(this.listBox1_Leave);
            // 
            // docListBox
            // 
            this.docListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.docListBox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.docListBox.FormattingEnabled = true;
            this.docListBox.Location = new System.Drawing.Point(1, 0);
            this.docListBox.Name = "docListBox";
            this.docListBox.Size = new System.Drawing.Size(315, 316);
            this.docListBox.TabIndex = 11;
            this.docListBox.Click += new System.EventHandler(this.docListBox_Click);
            this.docListBox.Enter += new System.EventHandler(this.docListBox_Enter);
            this.docListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.docListBox_KeyDown);
            this.docListBox.Leave += new System.EventHandler(this.docListBox_Leave);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 318);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(316, 180);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 32);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rasterPictureBox1);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.docListBox);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(712, 498);
            this.splitContainer1.SplitterDistance = 388;
            this.splitContainer1.TabIndex = 13;
            // 
            // UC_ContextDocumentViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.listBox1);
            this.Name = "UC_ContextDocumentViewer";
            this.Size = new System.Drawing.Size(801, 542);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonShowTifTags;
        private RasterPictureBoxWrapper rasterPictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox docListBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox AutoVisCheckBox;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
