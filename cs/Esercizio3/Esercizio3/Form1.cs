using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esercizio3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            for (int i = 0; i <= 10; i++)
            {
                listBox1.Items.Add(i);
            }
            int j = Int32.Parse("42");
            MessageBox.Show("Hello World");
            textBox1.AppendText("j = " + j);            

        }
    }
}
