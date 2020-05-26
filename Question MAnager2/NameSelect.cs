using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Question_MAnager2
{
    public partial class NameSelect : Form
    {
        public string name;

        public NameSelect(string text)
        {
            InitializeComponent();
            name = null;
            Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox.Text;
            Close();
        }
    }
}
