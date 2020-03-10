using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2020_03_03_2k_AnimationDblBuf
{
    public partial class Form1 : Form
    {
        private Animator a;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(mainPanel.CreateGraphics(), mainPanel.ClientRectangle);
        }

        private void mainPanel_Click(object sender, EventArgs e)
        {
            a.Start();
        }

        private void mainPanel_Resize(object sender, EventArgs e)
        {
            a.Update(mainPanel.CreateGraphics(), 
                mainPanel.ClientRectangle
            );
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.Stop();
        }
    }
}
