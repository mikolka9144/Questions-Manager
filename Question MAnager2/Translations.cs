using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points_submit
{
    public class Translations
    {
        public List<string> QuestionDesc;
        public List<string> LessonDesc;
        public Translations()
        {
            var enter = Environment.NewLine.ToCharArray().First();
            QuestionDesc = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions_translation.tsv").Split(enter).ToList();
            LessonDesc = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories_translation.tsv").Split(enter).ToList();
        }
        public void AddQuestionDesc(string Guid,string desc)
        {
            QuestionDesc.Add($"{Guid}\t\t\t{desc}\t{desc}\t{desc}");
        }
        public void RemoveQuestionDesc(string guid)
        {
            QuestionDesc.Remove(QuestionDesc.Find(s => s.Contains(guid)));
        }
        public void AddLessonDesc(string name)
        {
            LessonDesc.Add($"\\custom\\{name}\t{name}\t{name}\t{name}");
        }
        public void RemoveLessonDesc(string name)
        {
            LessonDesc.Remove(LessonDesc.Find(s => s.Contains($"\\custom\\{name}")));
        }
        public void Save()
        {
            string categories = "";
            foreach (var item in LessonDesc)
            {
                categories = categories + item + Environment.NewLine;
            }
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories_translation.tsv",categories);
            string questions = "";
            foreach (var item in QuestionDesc)
            {
                questions = questions + item + Environment.NewLine;
            }
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions_translation.tsv", questions);
        }
    }
}
