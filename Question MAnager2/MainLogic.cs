using PixBlocks.DataModels.Questions;
using PixBlocks.Tools.StringCompressor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Points_submit
{
    public class MainLogic
    {
        public MainLogic(UIRef ui, List<Question> questions, List<QuestionCategory> categories, Translations translations,QuestionNamer namer)
        {
            Ui = ui;
            this.questions = questions;
            this.categories = categories;
            this.translations = translations;
            this.namer = namer;
            custom = categories.Find(s => s.CategoryUserFriendlyName == "custom");
        }
        Translations translations;
        private readonly QuestionNamer namer;
        private QuestionCategory custom;
        private List<QuestionCategory> customs { 
            get
            {
                var list = new List<QuestionCategory>();
                foreach (var item in custom.SubcategoriesUniquePaths)
                {
                   
                    list.Add(categories.Find(s => s.UniquePath == item));
                }
                list.Remove(list.Find(s => s.UniquePath == "\\custom\\"));
                return list;
            }
        }
        private List<Question> questions;
        private List<QuestionCategory> categories;

        public UIRef Ui { get; }

        public void CreateNewLesson()
        {
            var category = new QuestionCategory();
            category.CategoryUserFriendlyName = Ui.GetName("Wpisz nazwę lekcji");
            category.UniquePath = $"\\custom\\{category.CategoryUserFriendlyName}";
            custom.SubcategoriesUniquePaths.Add($"\\custom\\{category.CategoryUserFriendlyName}");
            categories.Add(category);
            translations.AddLessonDesc(category.CategoryUserFriendlyName);
        }
        private Question LoadQuestion()
        {
            try
            {
                return (Question)new XmlSerializer(typeof(Question)).Deserialize(File.OpenRead(Ui.SelectFile()));

            }
            catch (Exception)
            {
                return null;
            }
        }
        public void AddQuestion()
        {
            QuestionCategory category;
            var questionToLoad = LoadQuestion();
            if (questionToLoad is null)
            {
                return;
            }
            category = Ui.GetLesson(customs);

            if (category is null)
            {
                Ui.message("Brak lekcji");
                return;
            }
            var name = Ui.GetName("Podaj nazwę pytania");

            Guid guid = Guid.NewGuid();
            category.SubQuestionsGuids.Add(guid.ToString());
            questionToLoad.UniqueGuid = guid.ToString();

            namer.list.Add((guid.ToString(), name));

            questions.Add(questionToLoad);
            translations.AddQuestionDesc(guid.ToString(), questionToLoad.Description);
            
        }

        

        public void SaveAll()
        {
            BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories.bin", categories);
            BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions.bin", questions);
            translations.Save();
            namer.Save();
        }
        public void ExportQuestion()
        {
            var category = Ui.GetCategory(customs);
            if (category is null)
            {
                return;
            }
            var ID = Ui.GetID(category);
            if (category.SubQuestionsGuids.Exists(s => s == ID))
            {
                new XmlSerializer(typeof(Question)).Serialize
                    (File.Create(AppDomain.CurrentDomain.BaseDirectory +$"{ID}.question")
                    ,questions.Find(s => s.UniqueGuid == ID));
                Ui.message("Export Ukończony");
                return;
            }
            Ui.message("Brak Pytania");
        }

        public void ExportCategory()
        {
            var category = new Category();
            category.category = Ui.GetCategory(customs);
            category.questions = new List<(Question,string)>(5);
            foreach (var item in category.category.SubQuestionsGuids)
            {
                category.questions.Add(
                    (questions.Find(s => s.UniqueGuid == item),
                    namer.list.Find((s) =>s.guid == item).val));
            }
            BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + $"{category.category.CategoryUserFriendlyName}.category",category);
            Ui.message("Export Ukończony");
        }

        public void ImportLesson()
        {
            var file = Ui.SelectFile();
            if (!File.Exists(file))
            {
                return;
            }
            var category = (Category)BinarySerializer.Deserialize(file);
            categories.Add(category.category);
            custom.SubcategoriesUniquePaths.Add(category.category.UniquePath);

            foreach (var item in category.questions)
            {
                questions.Add(item.Item1);
                namer.list.Add((item.Item1.UniqueGuid, item.Item2));
                translations.AddQuestionDesc(item.Item1.UniqueGuid, item.Item1.Description);
            }
            var name = category.category.UniquePath.Split('\\').Last();
            translations.AddLessonDesc(name);
            Ui.message("Dodano lekcję");
        }

        public void RemoveLesson()
        {
            var category = Ui.GetCategory(customs);
            if (category is null)
            {
                return;
            }
            foreach (var item in category.SubQuestionsGuids)
            {
                questions.Remove(questions.Find(s => s.UniqueGuid == item));
                namer.list.Remove(namer.list.Find(s => s.guid == item));
                translations.RemoveQuestionDesc(item);
            }
            categories.Remove(category);
            custom.SubcategoriesUniquePaths.Remove($"\\custom\\{category.CategoryUserFriendlyName}");
            translations.RemoveLessonDesc(category.CategoryUserFriendlyName);
            Ui.message("Usunięto kategorie");
        }

        public void EditQuestion()
        {
            var category = Ui.GetCategory(customs);
            if (category is null)
            {
                return;
            }
            var ID = Ui.GetID(category);

            var questionToLoad = LoadQuestion();
            if (questionToLoad is null)
            {
                return;
            }          

            if (category.SubQuestionsGuids.Exists(s => s == ID))
            {
                var oldQuestion = questions.Find(s => s.UniqueGuid == ID);
                questions.Remove(oldQuestion);
                questionToLoad.UniqueGuid = oldQuestion.UniqueGuid;
                questionToLoad.UserFriendlyName = oldQuestion.UserFriendlyName;
                questions.Add(questionToLoad);
                translations.RemoveQuestionDesc(oldQuestion.UniqueGuid);
                translations.AddQuestionDesc(oldQuestion.UniqueGuid, questionToLoad.Description);
                Ui.message("Edytowano pytanie");
            }
            else
            {
                Ui.message("Brak pytania");
            }
        }

        public void RemoveQuestion()
        {
            var category = Ui.GetCategory(customs);
            if (category is null)
            {
                return;
            }
            var ID = Ui.GetID(category);
            if (category.SubQuestionsGuids.Exists(s => s == ID))
            {
                category.SubQuestionsGuids.Remove(ID);
                questions.Remove(questions.Find(s => s.UniqueGuid == ID));
                namer.list.Remove(namer.list.Find(s => s.guid == ID));
                translations.RemoveQuestionDesc(ID);
                Ui.message("Usunieto pytane");
            }
            else
            {
                Ui.message("Brak pytania");
            }
        }        
    }
}
