namespace Ada.UI.Winforms.User_Controls
{
    partial class UC_WebBrowser
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BrowserPrintButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BrowserGoForwardButton = new System.Windows.Forms.Button();
            this.BrowserGoBackButton = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BrowserPrintButton);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.BrowserGoForwardButton);
            this.panel1.Controls.Add(this.BrowserGoBackButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(903, 40);
            this.panel1.TabIndex = 3;
            // 
            // BrowserPrintButton
            // 
            this.BrowserPrintButton.Image = global::Ada.Properties.Resources.printer;
            this.BrowserPrintButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BrowserPrintButton.Location = new System.Drawing.Point(14, 8);
            this.BrowserPrintButton.Name = "BrowserPrintButton";
            this.BrowserPrintButton.Size = new System.Drawing.Size(67, 23);
            this.BrowserPrintButton.TabIndex = 4;
            this.BrowserPrintButton.Text = "Udskriv";
            this.BrowserPrintButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BrowserPrintButton.UseVisualStyleBackColor = true;
            this.BrowserPrintButton.Click += new System.EventHandler(this.BrowserPrintButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(240, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(227, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(480, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(414, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDoubleClick);
            // 
            // BrowserGoForwardButton
            // 
            this.BrowserGoForwardButton.Enabled = false;
            this.BrowserGoForwardButton.Image = global::Ada.Properties.Resources.browse_right;
            this.BrowserGoForwardButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BrowserGoForwardButton.Location = new System.Drawing.Point(164, 8);
            this.BrowserGoForwardButton.Name = "BrowserGoForwardButton";
            this.BrowserGoForwardButton.Size = new System.Drawing.Size(55, 23);
            this.BrowserGoForwardButton.TabIndex = 1;
            this.BrowserGoForwardButton.Text = "Frem";
            this.BrowserGoForwardButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BrowserGoForwardButton.UseVisualStyleBackColor = true;
            this.BrowserGoForwardButton.Click += new System.EventHandler(this.BrowserGoForwardButton_Click);
            // 
            // BrowserGoBackButton
            // 
            this.BrowserGoBackButton.Enabled = false;
            this.BrowserGoBackButton.Image = global::Ada.Properties.Resources.browse_left;
            this.BrowserGoBackButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BrowserGoBackButton.Location = new System.Drawing.Point(87, 8);
            this.BrowserGoBackButton.Name = "BrowserGoBackButton";
            this.BrowserGoBackButton.Size = new System.Drawing.Size(71, 23);
            this.BrowserGoBackButton.TabIndex = 0;
            this.BrowserGoBackButton.Text = "Tilbage";
            this.BrowserGoBackButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BrowserGoBackButton.UseVisualStyleBackColor = true;
            this.BrowserGoBackButton.Click += new System.EventHandler(this.BrowserGoBackButton_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 40);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(903, 606);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // UC_WebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panel1);
            this.Name = "UC_WebBrowser";
            this.Size = new System.Drawing.Size(903, 646);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BrowserGoForwardButton;
        private System.Windows.Forms.Button BrowserGoBackButton;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BrowserPrintButton;
    }
}
