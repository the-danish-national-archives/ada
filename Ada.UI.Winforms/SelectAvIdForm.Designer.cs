//namespace Ada.UI.Winforms
//{
//    partial class SelectAvIdForm
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;
//
//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//
//        #region Windows Form Designer generated code
//
//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.startButton = new System.Windows.Forms.Button();
//            this.browseAV = new System.Windows.Forms.Button();
//            this.textBoxAV = new System.Windows.Forms.TextBox();
//            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
//            this.groupBox_Select_Arkiveringsversion = new System.Windows.Forms.GroupBox();
//            this.groupBox_Select_Arkiveringsversion.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // startButton
//            // 
//            this.startButton.Location = new System.Drawing.Point(15, 49);
//            this.startButton.Name = "startButton";
//            this.startButton.Size = new System.Drawing.Size(95, 23);
//            this.startButton.TabIndex = 9;
//            this.startButton.Text = "Start test";
//            this.startButton.UseVisualStyleBackColor = true;
//            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
//            // 
//            // browseAV
//            // 
//            this.browseAV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.browseAV.Location = new System.Drawing.Point(308, 23);
//            this.browseAV.Name = "browseAV";
//            this.browseAV.Size = new System.Drawing.Size(27, 21);
//            this.browseAV.TabIndex = 11;
//            this.browseAV.Text = "...";
//            this.browseAV.UseVisualStyleBackColor = true;
//            this.browseAV.Click += new System.EventHandler(this.BrowseAV_Click);
//            // 
//            // textBoxAV
//            // 
//            this.textBoxAV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.textBoxAV.Location = new System.Drawing.Point(15, 23);
//            this.textBoxAV.Name = "textBoxAV";
//            this.textBoxAV.Size = new System.Drawing.Size(287, 20);
//            this.textBoxAV.TabIndex = 10;
//            this.textBoxAV.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxAV_KeyUp);
//            // 
//            // groupBox_Select_Arkiveringsversion
//            // 
//            this.groupBox_Select_Arkiveringsversion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.groupBox_Select_Arkiveringsversion.Controls.Add(this.startButton);
//            this.groupBox_Select_Arkiveringsversion.Controls.Add(this.browseAV);
//            this.groupBox_Select_Arkiveringsversion.Controls.Add(this.textBoxAV);
//            this.groupBox_Select_Arkiveringsversion.Location = new System.Drawing.Point(12, 12);
//            this.groupBox_Select_Arkiveringsversion.Name = "groupBox_Select_Arkiveringsversion";
//            this.groupBox_Select_Arkiveringsversion.Size = new System.Drawing.Size(353, 87);
//            this.groupBox_Select_Arkiveringsversion.TabIndex = 9;
//            this.groupBox_Select_Arkiveringsversion.TabStop = false;
//            this.groupBox_Select_Arkiveringsversion.Text = "Arkiveringsversion";
//            // 
//            // SelectAvIdForm
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(377, 114);
//            this.Controls.Add(this.groupBox_Select_Arkiveringsversion);
//            this.Name = "SelectAvIdForm";
//            this.Text = "SelectAvId";
//            this.Load += new System.EventHandler(this.SelectAvId_Load);
//            this.groupBox_Select_Arkiveringsversion.ResumeLayout(false);
//            this.groupBox_Select_Arkiveringsversion.PerformLayout();
//            this.ResumeLayout(false);
//
//        }
//
//        #endregion
//
//        private System.Windows.Forms.GroupBox groupBox_Select_Arkiveringsversion;
//        private System.Windows.Forms.Button startButton;
//        private System.Windows.Forms.Button browseAV;
//        private System.Windows.Forms.TextBox textBoxAV;
//        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
//    }
//}