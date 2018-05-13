using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CsGL.OpenGL;
using System.Runtime.InteropServices;

namespace arcball
{
    public partial class Net_Analysis: Form
    {
        public Delaunay_2D de;
        public container c;
        int num;
        public Net_Analysis(int step, container c1)
        {
            c=c1;
            num = step;
            InitializeComponent();
            ogl1.Dock = DockStyle.Fill;
        }
        private void Net_Analysis_SizeChanged(object sender, EventArgs e)
        {
            ogl1.PlotGL();
        }
        private void Net_Analysis_Load(object sender, EventArgs e)
        {
            util u1 = new util();
            //清空相关文件内容
            u1.Initial_File();
            c.START_Inmodel(ogl1);
            ogl1.PlotGL();
            de = new Delaunay_2D();
            de.GetOGL(ogl1);
            de.Get_initialpl(c.initial_pl);
            de.Get_initial_center(c.center);
            de.GetPL(c.Get_p_l());
            de.Start_step(num);
            ogl1.PlotGL();
            list_box_add();
            //add_text();
            this.KeyPreview = true;
        }
        private void list_box_add()
        {
            LinkList<PO> plt = de.work_pl;
            Node<PO> pnt = plt.Head;
            int i = 0;
            listBox1.Items.Clear();
            listBox1.Items.Add("---"+(de.loc)+"---");
            while (pnt != null)
            {
                listBox1.Items.Add(":"+i+'('+pnt.Data.x+','+pnt.Data.y+','+pnt.Data.z+')');
                i++;
                pnt = pnt.Next;
            }
        }
        private void list(int num,Delaunay_2D de0)
        {
            Node<PO> pnt = de0.work_pl.Head;
            for(int i=0;i<num;i++)
            {
                if (pnt != null)
                    pnt = pnt.Next;
                else
                {
                    MessageBox.Show("null is appearing!!");
                    return;
                }
            }
            if (listBox2.Items.Count > 0)
                listBox2.Items.Clear();
            Node<LINE> lnt = pnt.Data.ll.Head;
            int j = 0;
            while (lnt != null)
            {
                listBox2.Items.Add(j+lnt.Data.ToString());
                j++;
                lnt = lnt.Next;
            }
            if(pnt.Data.p_parent!=null)
                listBox2.Items.Add("p_parent:"+pnt.Data.p_parent.ToString());
            else
                listBox2.Items.Add("p_parent:" + "--------------");
            if(pnt.Data.selected)
                listBox2.Items.Add("selected");
            else
                listBox2.Items.Add("not selected");
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            list(listBox1.SelectedIndex-1, de);
            util u1 = new util();
            PO[] pnc;
            pnc = u1.Get_po_from_string(listBox1.SelectedItem.ToString());
            ogl1.p_tags = pnc[0];
            ogl1.PlotGLKey();
        }
        private void add_text()
        {
            Node<String> st = de.info.Head;
            listBox3.Items.Clear();
            while (st != null)
            {
                listBox3.Items.Add(st.Data);
                st = st.Next;
            }
            
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            util u1 = new util();
            PO[] pnc;
            pnc=u1.Get_po_from_string(listBox2.SelectedItem.ToString());
            ogl1.l_tag = new LINE(pnc[0],pnc[1]);
            ogl1.PlotGLKey();
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

        private void pEBIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_PEBI = true;
            ogl1.SHOW_TRIANGLE = false;
            ogl1.PlotGLKey();
        }

        private void tRIANGLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_PEBI = false;
            ogl1.SHOW_TRIANGLE = true; ;
            ogl1.PlotGLKey();
        }

        private void generatedpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_ORDINARY_POINT = true;
            ogl1.PlotGLKey();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_ORDINARY_POINT = false;
            ogl1.PlotGLKey();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            de.SearchPoint_next_step();
            ogl1.PlotGLKey();
            list_box_add();
            //add_text();
        }

        private void pebiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_PEBI = true;
            ogl1.PlotGLKey();
        }

        private void triangleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ogl1.SHOW_TRIANGLE = true;
            ogl1.PlotGLKey();
        }

        private void addtextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            util u1=new util();
            Node<String> sn = de.info.Head;
            FileStream stream = File.Open(u1.infopath, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();//清空文件内容
            StreamWriter sw = new StreamWriter(u1.infopath,true);
            while (sn != null)
            {
                sw.WriteLine(sn.Data);
                sn = sn.Next;
            }
            sw.Close();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            util u1 = new util();
            Node<PO> pn = de.work_pl.Head;
            while (pn != null)
            {
                if(!u1.Test_l_l(pn.Data.ll,pn.Data)) pn.Data.key=5;
                pn = pn.Next;
            }
            ogl1.PlotGLKey();
        }
    }
}
