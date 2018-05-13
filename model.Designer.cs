namespace arcball
{
    partial class model
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(model));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.ogl1 = new OGL();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.netAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.netAnalysisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.netcreateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showmodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showgeneratedpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showeditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showtogetherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showtriangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showpebiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.ouputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 32);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1320, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "vertical well";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton2.Text = "fault";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton3.Text = "horizontal well";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton4.Text = "stop";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // ogl1
            // 
            this.ogl1.Location = new System.Drawing.Point(14, 70);
            this.ogl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ogl1.Name = "ogl1";
            this.ogl1.Size = new System.Drawing.Size(1294, 898);
            this.ogl1.TabIndex = 4;
            this.ogl1.Text = "ogl1";
            this.ogl1.Click += new System.EventHandler(this.ogl1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.netAnalysisToolStripMenuItem,
            this.netcreateToolStripMenuItem,
            this.showmodeToolStripMenuItem,
            this.ouputToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1320, 32);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // netAnalysisToolStripMenuItem
            // 
            this.netAnalysisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.netAnalysisToolStripMenuItem1});
            this.netAnalysisToolStripMenuItem.Name = "netAnalysisToolStripMenuItem";
            this.netAnalysisToolStripMenuItem.Size = new System.Drawing.Size(56, 28);
            this.netAnalysisToolStripMenuItem.Text = "tool";
            // 
            // netAnalysisToolStripMenuItem1
            // 
            this.netAnalysisToolStripMenuItem1.Name = "netAnalysisToolStripMenuItem1";
            this.netAnalysisToolStripMenuItem1.Size = new System.Drawing.Size(202, 30);
            this.netAnalysisToolStripMenuItem1.Text = "Net_Analysis";
            this.netAnalysisToolStripMenuItem1.Click += new System.EventHandler(this.netAnalysisToolStripMenuItem1_Click);
            // 
            // netcreateToolStripMenuItem
            // 
            this.netcreateToolStripMenuItem.Name = "netcreateToolStripMenuItem";
            this.netcreateToolStripMenuItem.Size = new System.Drawing.Size(111, 28);
            this.netcreateToolStripMenuItem.Text = "net_create";
            this.netcreateToolStripMenuItem.Click += new System.EventHandler(this.netcreateToolStripMenuItem_Click);
            // 
            // showmodeToolStripMenuItem
            // 
            this.showmodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showgeneratedpointToolStripMenuItem,
            this.showeditToolStripMenuItem,
            this.showtogetherToolStripMenuItem,
            this.showtriangleToolStripMenuItem,
            this.showpebiToolStripMenuItem});
            this.showmodeToolStripMenuItem.Name = "showmodeToolStripMenuItem";
            this.showmodeToolStripMenuItem.Size = new System.Drawing.Size(124, 28);
            this.showmodeToolStripMenuItem.Text = "show_mode";
            this.showmodeToolStripMenuItem.Click += new System.EventHandler(this.showmodeToolStripMenuItem_Click);
            // 
            // showgeneratedpointToolStripMenuItem
            // 
            this.showgeneratedpointToolStripMenuItem.Name = "showgeneratedpointToolStripMenuItem";
            this.showgeneratedpointToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.showgeneratedpointToolStripMenuItem.Text = "show_generated_point";
            this.showgeneratedpointToolStripMenuItem.Click += new System.EventHandler(this.showgeneratedpointToolStripMenuItem_Click);
            // 
            // showeditToolStripMenuItem
            // 
            this.showeditToolStripMenuItem.Name = "showeditToolStripMenuItem";
            this.showeditToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.showeditToolStripMenuItem.Text = "show_edit";
            this.showeditToolStripMenuItem.Click += new System.EventHandler(this.showeditToolStripMenuItem_Click);
            // 
            // showtogetherToolStripMenuItem
            // 
            this.showtogetherToolStripMenuItem.Name = "showtogetherToolStripMenuItem";
            this.showtogetherToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.showtogetherToolStripMenuItem.Text = "show_together";
            this.showtogetherToolStripMenuItem.Click += new System.EventHandler(this.showtogetherToolStripMenuItem_Click);
            // 
            // showtriangleToolStripMenuItem
            // 
            this.showtriangleToolStripMenuItem.Name = "showtriangleToolStripMenuItem";
            this.showtriangleToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.showtriangleToolStripMenuItem.Text = "show_triangle";
            this.showtriangleToolStripMenuItem.Click += new System.EventHandler(this.showtriangleToolStripMenuItem_Click);
            // 
            // showpebiToolStripMenuItem
            // 
            this.showpebiToolStripMenuItem.Name = "showpebiToolStripMenuItem";
            this.showpebiToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.showpebiToolStripMenuItem.Text = "show_pebi ";
            this.showpebiToolStripMenuItem.Click += new System.EventHandler(this.showpebiToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(879, 673);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 28);
            this.button1.TabIndex = 6;
            this.button1.Text = "[]";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ouputToolStripMenuItem
            // 
            this.ouputToolStripMenuItem.Name = "ouputToolStripMenuItem";
            this.ouputToolStripMenuItem.Size = new System.Drawing.Size(74, 28);
            this.ouputToolStripMenuItem.Text = "ouput";
            this.ouputToolStripMenuItem.Click += new System.EventHandler(this.ouputToolStripMenuItem_Click);
            // 
            // model
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 981);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ogl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "model";
            this.Text = "_3Dshow";
            this.Load += new System.EventHandler(this.model_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private OGL ogl1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem netAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem netAnalysisToolStripMenuItem1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem netcreateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showmodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showgeneratedpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showeditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showtogetherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showtriangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem showpebiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ouputToolStripMenuItem;





    }
}