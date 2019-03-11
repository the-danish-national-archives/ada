using Ra.DocumentInvestigator.UsercontrolsWrappers;

namespace Ada.UI.Winforms.User_Controls
{
    partial class UC_DocumentViewerForm
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rasterImageViewer1 = new RasterImageViewerWrapper();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(513, 33);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Normal";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(84, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Sidebredde";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(165, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Tilpasset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rasterImageViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 583);
            this.panel1.TabIndex = 2;
            // 
            // rasterImageViewer1
            // 
            this.rasterImageViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rasterImageViewer1.EnableScrollingInterface = true;
            this.rasterImageViewer1.HorizontalAlignMode = "Center";
            this.rasterImageViewer1.Location = new System.Drawing.Point(0, 0);
            this.rasterImageViewer1.Name = "rasterImageViewer1";
            this.rasterImageViewer1.Size = new System.Drawing.Size(513, 583);
            this.rasterImageViewer1.TabIndex = 1;
            this.rasterImageViewer1.VerticalAlignMode = "Center";
            this.rasterImageViewer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rasterImageViewer1_MouseMove);
            this.rasterImageViewer1.ScaleFactorChanged += new System.EventHandler(this.rasterImageViewer1_ScaleFactorChanged);
            this.rasterImageViewer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rasterImageViewer1_MouseDown);
            this.rasterImageViewer1.Resize += new System.EventHandler(this.rasterImageViewer1_Resize);
            this.rasterImageViewer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rasterImageViewer1_MouseUp);
            this.rasterImageViewer1.ImageChanged += new System.EventHandler(this.rasterImageViewer1_ImageChanged);
            // 
            // UC_DocumentViewerForm
            // 
            this.AccessibleName = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 616);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "UC_DocumentViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImgViewForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ImgViewForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImgViewForm_FormClosing);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
        public RasterImageViewerWrapper rasterImageViewer1;
    }
}