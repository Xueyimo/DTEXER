namespace arcball
{
    partial class showplt
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
        private void InitializeComponent(OGL ogl)
        {
            this.ogl1 = ogl;
            this.SuspendLayout();
            // 
            // ogl1
            // 
            this.ogl1.Location = new System.Drawing.Point(41, 35);
            this.ogl1.Name = "ogl1";
            this.ogl1.Size = new System.Drawing.Size(136, 139);
            this.ogl1.TabIndex = 0;
            this.ogl1.Text = "ogl1";
            this.ogl1.Click += new System.EventHandler(this.ogl1_Click);
            // 
            // FormMaxSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.ogl1);
            this.Name = "FormMaxSize";
            this.Text = "FormMaxSize";
            this.Load += new System.EventHandler(this.FormMaxSize_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OGL ogl1;
    }
}