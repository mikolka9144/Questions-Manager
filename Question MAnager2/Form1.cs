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
    public partial class Main : Form
    {
        public Main(MainLogic logic)
        {
            InitializeComponent();
            Logic = logic;
        }

        public MainLogic Logic { get; }

        private void AddQuestion(object sender, EventArgs e)
        {
            Lock();
            Logic.AddQuestion();
            Unlock();
        }

        private void Lock()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void Unlock()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
        }

        private void RemoveQuestion(object sender, EventArgs e)
        {
            Lock();
            Logic.RemoveQuestion();
            Unlock();
        }

        private void EditQuestion(object sender, EventArgs e)
        {
            Lock();
            Logic.EditQuestion();
            Unlock();
        }

        private void ExportQuestion(object sender, EventArgs e)
        {
            Lock();
            Logic.ExportQuestion();
            Unlock();
        }

        private void RemoveCategory(object sender, EventArgs e)
        {
            Lock();
            Logic.RemoveLesson();
            Unlock();
        }

        private void ExportCategory(object sender, EventArgs e)
        {
            Lock();
            Logic.ExportCategory();
            Unlock();
        }

        private void ImportCategory(object sender, EventArgs e)
        {
            Lock();
            Logic.ImportLesson();
            Unlock();
        }

        private void CreateNewLesson(object sender, EventArgs e)
        {
            Lock();
            Logic.CreateNewLesson();
            Unlock();
        }

        private void Save(object sender, EventArgs e)
        {
            Lock();
            Logic.SaveAll();
            Unlock();
        }
    }
}
