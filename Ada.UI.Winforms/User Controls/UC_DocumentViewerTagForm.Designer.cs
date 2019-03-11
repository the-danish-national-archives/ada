namespace Ada.UI.Winforms.User_Controls
{
    partial class UC_DocumentViewerTagForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonGemSom = new System.Windows.Forms.Button();
            this.DataBut = new System.Windows.Forms.Button();
            this.GroupBut = new System.Windows.Forms.Button();
            this.HintBut = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.buttonPrint);
            this.panel1.Controls.Add(this.buttonGemSom);
            this.panel1.Controls.Add(this.DataBut);
            this.panel1.Controls.Add(this.GroupBut);
            this.panel1.Controls.Add(this.HintBut);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(446, 40);
            this.panel1.TabIndex = 3;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(350, 7);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonPrint.TabIndex = 4;
            this.buttonPrint.Text = "Udskriv";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // buttonGemSom
            // 
            this.buttonGemSom.Location = new System.Drawing.Point(269, 7);
            this.buttonGemSom.Name = "buttonGemSom";
            this.buttonGemSom.Size = new System.Drawing.Size(75, 23);
            this.buttonGemSom.TabIndex = 3;
            this.buttonGemSom.Text = "Gem som ...";
            this.buttonGemSom.UseVisualStyleBackColor = true;
            this.buttonGemSom.Click += new System.EventHandler(this.buttonGemSom_Click);
            // 
            // DataBut
            // 
            this.DataBut.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.DataBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DataBut.Location = new System.Drawing.Point(3, 7);
            this.DataBut.Name = "DataBut";
            this.DataBut.Size = new System.Drawing.Size(75, 23);
            this.DataBut.TabIndex = 2;
            this.DataBut.Text = "Data";
            this.DataBut.UseVisualStyleBackColor = false;
            this.DataBut.Click += new System.EventHandler(this.DataBut_Click);
            // 
            // GroupBut
            // 
            this.GroupBut.Location = new System.Drawing.Point(166, 7);
            this.GroupBut.Name = "GroupBut";
            this.GroupBut.Size = new System.Drawing.Size(75, 23);
            this.GroupBut.TabIndex = 1;
            this.GroupBut.Text = "Gruppe";
            this.GroupBut.UseVisualStyleBackColor = true;
            this.GroupBut.Click += new System.EventHandler(this.GroupBut_Click);
            // 
            // HintBut
            // 
            this.HintBut.Location = new System.Drawing.Point(84, 7);
            this.HintBut.Name = "HintBut";
            this.HintBut.Size = new System.Drawing.Size(75, 23);
            this.HintBut.TabIndex = 0;
            this.HintBut.Text = "Forklaring";
            this.HintBut.UseVisualStyleBackColor = true;
            this.HintBut.Click += new System.EventHandler(this.HintBut_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8F);
            this.richTextBox1.Location = new System.Drawing.Point(1, 38);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(446, 492);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Rich Text Format|*.rtf|All files|*.*";
            // 
            // UC_DocumentViewerTagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 531);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "UC_DocumentViewerTagForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Viewer";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TagViewForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonGemSom;
        private System.Windows.Forms.Button DataBut;
        private System.Windows.Forms.Button GroupBut;
        private System.Windows.Forms.Button HintBut;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}