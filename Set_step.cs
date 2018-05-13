using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace arcball
{
    public partial class Set_step : Form
    {
        public int step;
        public container c;
        public Set_step( container c1)
        {
            c = c1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            step = int.Parse(textBox1.Text);
            Net_Analysis childForm = new Net_Analysis(step,c);
            childForm.WindowState = FormWindowState.Maximized;//窗体最大化
            childForm.Show();
            childForm.Dock = DockStyle.Fill;
        }
    }
}
