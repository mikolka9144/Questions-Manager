using PixBlocks.DataModels.Questions;
using Question_MAnager2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Points_submit
{
    public class UIRef
    {
        private QuestionNamer namer;

        public UIRef(QuestionNamer namer)
        {
            this.namer = namer;
        }
        internal void message(string v)
        {
            MessageBox.Show(v);
        }

        public QuestionCategory GetCategory(IEnumerable<QuestionCategory> categories)
        {
            var lesson = GetLesson(categories);
            if (lesson is null)
            {
                message("Brak lekcji");
                return null;
            }
            return lesson;
        }

        public string GetID(QuestionCategory category)
        {
            var list = new List<string>();
            foreach (var item in category.SubQuestionsGuids)
            {
                list.Add(namer.list.Find(s => s.guid == item).val);
            }
            var box = new GetItem(list,"Wybierz pytanie");
            box.ShowDialog();
            return namer.list.Find(s => s.val ==  box.item).guid;
        }

        public QuestionCategory GetLesson(IEnumerable<QuestionCategory> lessons)
        {
            var listOfNames = new List<string>();
            foreach (var item in lessons)
            {
                listOfNames.Add(item.CategoryUserFriendlyName);
            }
            var box = new GetItem(listOfNames, "Wybierz lekcję");
            box.ShowDialog();
            return lessons.FirstOrDefault(s => s.CategoryUserFriendlyName == box.item);
        }

        internal string SelectFile(string cap = "Wybierz plik")
        {
            var file = new OpenFileDialog();
            file.Title = cap;
            file.ShowDialog();
            return file.FileName;
        }

        internal string GetName(string text)
        {
            var textSelector = new NameSelect(text);
            textSelector.ShowDialog();
            return textSelector.name;
        }
    }
}