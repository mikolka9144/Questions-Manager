using PixBlocks.DataModels.Questions;
using Points_submit;
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
    public partial class GetItem : Form
    {
        public string item;
        public GetItem(IEnumerable<string> items,string cap)
        {
            InitializeComponent();
            Text = cap;
            item = "";
                foreach (var item in items)
                {
                    listOfGuids.Items.Add(item);
                }
          
        }

        public QuestionNamer Namer { get; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Namer != null)
                {
                    var name = listOfGuids.SelectedItems[0].Text;
                    item = Namer.list.Find(s => s.val == name).guid;
                }
                else
                {
                    item = listOfGuids.SelectedItems[0].Text;
                }

            }
            catch (Exception)
            {
            }
            Close();
        }
    }
}
