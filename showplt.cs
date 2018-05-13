using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CsGL.OpenGL;
using System.IO;

namespace arcball
{
    public partial class showplt : Form
    {
        Vector3f p_tag = new Vector3f();//选取的点
        public showplt(OGL ogl)
        {
            InitializeComponent(ogl);
            ogl1.MouseMove += new System.Windows.Forms.MouseEventHandler(ogl1.glOnMouseMove);
            ogl1.Dock = DockStyle.Fill;
            this.KeyDown += _3Dshow_KeyDown;
            this.MouseWheel += _3Dshow_MouseWheel;
        }
        void _3Dshow_KeyDown(object sender, KeyEventArgs e)
        {
            //if escape was pressed, exit the application
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
            //if R was pressed, reset
            if (e.KeyCode == Keys.R)
            {
                ogl1.reset();
            }
        }

        void _3Dshow_MouseWheel(object sender, MouseEventArgs e)
        {

            float scale = 1.0f;
            if (e.Button != MouseButtons.Middle)
                if (e.Delta > 0)
                {
                    scale = 0.9f;
                }
                else
                {
                    scale = 1.1f;
                }
            GL.glScalef(scale, scale, scale);
            ogl1.PlotGLKey();
        }
        private void FormMaxSize_Load(object sender, EventArgs e)
        {
            ogl1.PlotGL();
        }

        private void ogl1_Click(object sender, EventArgs e)
        {
            ogl1.Focus();
        }
    }
}
