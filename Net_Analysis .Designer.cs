namespace arcball
{
    partial class Net_Analysis
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
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.ogl1 = new OGL();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.showmodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pEBIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tRIANGLEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatedpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pebiToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addtextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(781, 36);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 31);
            this.button1.TabIndex = 1;
            this.button1.Text = "MaxSize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.91564F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.08436F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ogl1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 61);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1873, 930);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.60419F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.39581F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listBox3, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(862, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(869, 922);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.listBox2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.listBox1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.03282F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.96718F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(459, 914);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 18;
            this.listBox2.Location = new System.Drawing.Point(3, 506);
            this.listBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(453, 400);
            this.listBox2.TabIndex = 1;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(3, 4);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(453, 490);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 18;
            this.listBox3.Location = new System.Drawing.Point(468, 3);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(398, 904);
            this.listBox3.TabIndex = 1;
            // 
            // ogl1
            // 
            this.ogl1.Location = new System.Drawing.Point(3, 4);
            this.ogl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ogl1.Name = "ogl1";
            this.ogl1.Size = new System.Drawing.Size(853, 922);
            this.ogl1.TabIndex = 1;
            this.ogl1.Text = "ogl1";
            this.ogl1.Click += new System.EventHandler(this.ogl1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showmodeToolStripMenuItem,
            this.addtextToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1899, 32);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // showmodeToolStripMenuItem
            // 
            this.showmodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pEBIToolStripMenuItem,
            this.tRIANGLEToolStripMenuItem,
            this.generatedpointToolStripMenuItem});
            this.showmodeToolStripMenuItem.Name = "showmodeToolStripMenuItem";
            this.showmodeToolStripMenuItem.Size = new System.Drawing.Size(124, 28);
            this.showmodeToolStripMenuItem.Text = "show_mode";
            // 
            // pEBIToolStripMenuItem
            // 
            this.pEBIToolStripMenuItem.Name = "pEBIToolStripMenuItem";
            this.pEBIToolStripMenuItem.Size = new System.Drawing.Size(235, 30);
            this.pEBIToolStripMenuItem.Text = "PEBI";
            this.pEBIToolStripMenuItem.Click += new System.EventHandler(this.pEBIToolStripMenuItem_Click);
            // 
            // tRIANGLEToolStripMenuItem
            // 
            this.tRIANGLEToolStripMenuItem.Name = "tRIANGLEToolStripMenuItem";
            this.tRIANGLEToolStripMenuItem.Size = new System.Drawing.Size(235, 30);
            this.tRIANGLEToolStripMenuItem.Text = "TRIANGLE";
            this.tRIANGLEToolStripMenuItem.Click += new System.EventHandler(this.tRIANGLEToolStripMenuItem_Click);
            // 
            // generatedpointToolStripMenuItem
            // 
            this.generatedpointToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.pebiToolStripMenuItem1,
            this.triangleToolStripMenuItem1});
            this.generatedpointToolStripMenuItem.Name = "generatedpointToolStripMenuItem";
            this.generatedpointToolStripMenuItem.Size = new System.Drawing.Size(235, 30);
            this.generatedpointToolStripMenuItem.Text = "generated_point";
            this.generatedpointToolStripMenuItem.Click += new System.EventHandler(this.generatedpointToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 30);
            this.exitToolStripMenuItem.Text = "exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pebiToolStripMenuItem1
            // 
            this.pebiToolStripMenuItem1.Name = "pebiToolStripMenuItem1";
            this.pebiToolStripMenuItem1.Size = new System.Drawing.Size(159, 30);
            this.pebiToolStripMenuItem1.Text = "pebi";
            this.pebiToolStripMenuItem1.Click += new System.EventHandler(this.pebiToolStripMenuItem1_Click);
            // 
            // triangleToolStripMenuItem1
            // 
            this.triangleToolStripMenuItem1.Name = "triangleToolStripMenuItem1";
            this.triangleToolStripMenuItem1.Size = new System.Drawing.Size(159, 30);
            this.triangleToolStripMenuItem1.Text = "triangle";
            this.triangleToolStripMenuItem1.Click += new System.EventHandler(this.triangleToolStripMenuItem1_Click);
            // 
            // addtextToolStripMenuItem
            // 
            this.addtextToolStripMenuItem.Name = "addtextToolStripMenuItem";
            this.addtextToolStripMenuItem.Size = new System.Drawing.Size(97, 28);
            this.addtextToolStripMenuItem.Text = "add_text";
            this.addtextToolStripMenuItem.Click += new System.EventHandler(this.addtextToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(700, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "->";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(54, 28);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // Net_Analysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1899, 1024);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Net_Analysis";
            this.Text = "Net_Analysis";
            this.Load += new System.EventHandler(this.Net_Analysis_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private OGL ogl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showmodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pEBIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tRIANGLEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatedpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem pebiToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem triangleToolStripMenuItem1;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ToolStripMenuItem addtextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;





    }
}