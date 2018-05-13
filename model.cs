using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using CsGL.OpenGL;

namespace arcball
{
    public partial class model : Form
    {
        container c1;
        Delaunay_2D de;
        private bool flush_end = true;
        public model()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            ogl1.MouseMove += new System.Windows.Forms.MouseEventHandler(ogl1.glOnMouseMove);
            ogl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(ogl1.glOnMouseWheel);
            ogl1.KeyDown += new System.Windows.Forms.KeyEventHandler(ogl1.glOnKeyDown);
            //ogl1.Dock = DockStyle.Fill;
        }
        private void model_SizeChanged(object sender, EventArgs e)
        {
            ogl1.PlotGL();
        }
        private void model_Load(object sender, EventArgs e)
        {
            util u1 = new util();
            //清空相关文件内容
            u1.Initial_File();
            c1 = new container();
            c1.GetOGL(ogl1);
            c1.START();
            ogl1.Set_model(c1);
            ogl1.PlotGL();
            this.KeyPreview = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            showplt fm = new showplt(ogl1);
            fm.Dock = DockStyle.Fill;
            fm.WindowState = FormWindowState.Maximized;
            fm.Show(); 
        }
        private void ogl1_Click(object sender, EventArgs e)
        {
            ogl1.Focus();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ogl1.ADD_MODEL_vwell = true;
            ogl1.ADD_MODEL_hwell = false;
            ogl1.ADD_MODEL_fault = false;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ogl1.ADD_MODEL_vwell = false;
            ogl1.ADD_MODEL_hwell = false;
            ogl1.ADD_MODEL_fault = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ogl1.ADD_MODEL_vwell = false;
            ogl1.ADD_MODEL_hwell = true;
            ogl1.ADD_MODEL_fault = false;
        }
        private void netAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Set_step childform = new Set_step(c1);
            c1.In_model();
            childform.WindowState = FormWindowState.Maximized;
            childform.Show();
        }
        private void netcreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_TRIANGLE = true;
            de = new Delaunay_2D();
            de.GetOGL(ogl1);
            de.Get_initialpl(c1.initial_pl);
            de.Get_initial_center(c1.center);
            de.GetPL(c1.Get_p_l());
            //Thread de_thread = new Thread(de.Start);
            //de_thread.Start();
            de.Start();
            ogl1.SHOW_PEBI = true;
            ogl1.SHOW_TRIANGLE = false;
            ogl1.SHOW_ORDINARY_POINT = false;
            ogl1.PlotGLKey();
        }
        private void Flush()
        {
            int i = 0;
            while (flush_end)
            {
                ogl1.PlotGLKey();
                Thread.Sleep(1000);
                if (++i > 2000) break;
            }
        }
        private void showgeneratedpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_ORDINARY_POINT = true;
            ogl1.SHOW_EDIT_MODEL = false;
            ogl1.PlotGLKey();
        }
        private void showeditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_EDIT_MODEL = true;
            ogl1.SHOW_ORDINARY_POINT = false;
            ogl1.PlotGLKey();
        }

        private void showmodeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showtogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_EDIT_MODEL = true;
            ogl1.SHOW_ORDINARY_POINT = true;
            ogl1.PlotGLKey();
        }

        private void showtriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_TRIANGLE = true;
            ogl1.SHOW_EDIT_MODEL = false;
            ogl1.SHOW_ORDINARY_POINT = false;
            ogl1.SHOW_PEBI = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            de.STOP = true;
        }

        private void showpebiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_PEBI = true;
            ogl1.SHOW_TRIANGLE = false;
        }

        private void ouputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            c1.Output_model_info();
        }
    }
}
