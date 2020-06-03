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
    public partial class Main : Form
    {
        public Main(MainLogic logic)
        {
            InitializeComponent();
            Logic = logic;
            ReloadList();
        }

        public MainLogic Logic { get; }

        private void ReloadList()
        {
            Categorieslist.Items.Clear();
            foreach (var item in Logic.customs)
            {
                var categoryToShow = new ListViewItem();
                categoryToShow.Text = item.CategoryUserFriendlyName;
                categoryToShow.Tag = item;
                Categorieslist.Items.Add(categoryToShow);
            }
        }
        private QuestionCategory getSelection()
        {
            if (Categorieslist.SelectedItems.Count == 1)
            {
                return (QuestionCategory)Categorieslist.SelectedItems[0].Tag;
            }
            return null;
        }
        private void RemoveCategory(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }
            Logic.RemoveLesson(category);
            ReloadList();
        }

        private void ExportCategory(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }
            Logic.ExportCategory(category);
        }

        private void ImportCategory(object sender, EventArgs e)
        {
            Logic.ImportLesson();
            ReloadList();
        }

        private void CreateNewLesson(object sender, EventArgs e)
        {
            Logic.CreateNewLesson();
            ReloadList();
        }

        private void Save(object sender, EventArgs e)
        {
            Logic.SaveAll();
        }

        private void Categorieslist_DoubleClick(object sender, EventArgs e)
        {
            var category = getSelection();
            if (category is null)
            {
                return;
            }
            var view = new LessonView(category, Logic);
            view.ShowDialog();
        }
    }
}
