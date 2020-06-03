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
    public partial class LessonView : Form
    {
        public string item;
        public LessonView(QuestionCategory category, MainLogic logic)
        {
            InitializeComponent();
            Category = category;
            Logic = logic;
            ReloadList();
        }

        public QuestionCategory Category { get; }
        public MainLogic Logic { get; }

        private void ReloadList()
        {
            Questionslist.Items.Clear();
            foreach (var item in Category.SubQuestionsGuids)
            {
                var categoryToShow = new ListViewItem();
                categoryToShow.Text = Logic.namer.list.Find(s => s.guid == item).val;
                categoryToShow.Tag = Logic.manager.questions.Find(s => s.UniqueGuid == item);
                Questionslist.Items.Add(categoryToShow);
            }
        }
        private Question getSelection()
        {
            if (Questionslist.SelectedItems.Count == 1)
            {
                return (Question)Questionslist.SelectedItems[0].Tag;
            }
            return null;
        }
        private void AddQuestion(object sender, EventArgs e)
        {
            Logic.AddQuestion(Category);
            ReloadList();
        }

        private void RemoveQuestion(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }
            Logic.RemoveQuestion(category,Category);
            ReloadList();
        }

        private void ExportQuestion(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }
            Logic.ExportQuestion(category);
        }

        private void EditQuestion(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }
            Logic.EditQuestion(category);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }

            Logic.Rename(category);
            ReloadList();
        }
    }
}
