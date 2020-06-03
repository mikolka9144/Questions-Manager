using PixBlocks.DataModels.Questions;
using PixBlocks.Tools.StringCompressor;
using Question_MAnager2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Points_submit
{
    public class MainLogic
    {
        public MainLogic(IUIRef ui, Translations translations,QuestionNamer namer,BinaryManager manager)
        {
            Ui = ui;
            this.translations = translations;
            this.namer = namer;
            this.manager = manager;

            if (!manager.categories.Any(s => s.CategoryUserFriendlyName == "custom"))
            {
                Ui.message("Konfiguracja programu");
                var core = new QuestionCategory();
                core.CategoryUserFriendlyName = "custom";
                core.UniquePath = "\\custom";
                translations.LessonDesc.Add("\\custom\tPytania Niestandartowe\tCustom Tasks\tНестандартные вопросы");
                manager.categories[0].SubcategoriesUniquePaths.Add("\\custom");
                manager.categories.Add(core);
                SaveAll();
            }

            custom = manager.categories.Find(s => s.CategoryUserFriendlyName == "custom");
        }

        Translations translations;
        public readonly QuestionNamer namer;
        public readonly BinaryManager manager;
        private QuestionCategory custom;

        public List<QuestionCategory> customs { 
            get
            {
                var list = new List<QuestionCategory>();
                foreach (var item in custom.SubcategoriesUniquePaths)
                {
                   
                    list.Add(manager.categories.Find(s => s.UniquePath == item));
                }
                return list;
            }
        }

        IUIRef Ui { get; }

        public void CreateNewLesson()
        {
            var category = new QuestionCategory();
            category.CategoryUserFriendlyName = Ui.GetName("Wpisz nazwę lekcji");
            category.UniquePath = $"\\custom\\{category.CategoryUserFriendlyName}";
            custom.SubcategoriesUniquePaths.Add($"\\custom\\{category.CategoryUserFriendlyName}");
            manager.categories.Add(category);
            translations.AddLessonDesc(category.CategoryUserFriendlyName);
        }
        private Question LoadQuestion()
        {
            try
            {
                return (Question)new XmlSerializer(typeof(Question)).Deserialize(File.OpenRead(Ui.SelectFile("Pytanie|*.question")));

            }
            catch (Exception x)
            {
                return null;
            }
        }

        public void Rename(Question question)
        {
            var NewName = Ui.GetName("Podaj nową nazwę pytania");
            if (NewName is null)
            {
                return;
            }
            namer.list.RemoveAll(s => s.guid == question.UniqueGuid);
            namer.list.Add((question.UniqueGuid, NewName));

        }

        public void AddQuestion(QuestionCategory category)
        {
            var questionToLoad = LoadQuestion();
            if (questionToLoad is null)
            {
                return;
            }
            var name = Ui.GetName("Podaj nazwę pytania");

            Guid guid = Guid.NewGuid();
            category.SubQuestionsGuids.Add(guid.ToString());
            questionToLoad.UniqueGuid = guid.ToString();

            namer.list.Add((guid.ToString(), name));

            manager.questions.Add(questionToLoad);
            translations.AddQuestionDesc(guid.ToString(), questionToLoad.Description);
            
        }

        

        public void SaveAll()
        {
            manager.Save();
            translations.Save();
            namer.Save();
        }
        public void ExportQuestion(Question question)
        {
            var fileName = AppDomain.CurrentDomain.BaseDirectory + $"{namer.list.Find(s => s.guid == question.UniqueGuid).val}.question";
            new XmlSerializer(typeof(Question)).Serialize(File.Create(fileName),question);
            Ui.message("Export Ukończony");
        }

        public void ExportCategory(QuestionCategory categoryPix)
        {
            var category = new Category();
            category.category = categoryPix;
            category.questions = new List<(Question,string)>(5);

            var filename = AppDomain.CurrentDomain.BaseDirectory + $"{categoryPix.CategoryUserFriendlyName}.category";

            foreach (var item in categoryPix.SubQuestionsGuids)
            {
                category.questions.Add(
                    (manager.questions.Find(s => s.UniqueGuid == item),
                    namer.list.Find((s) =>s.guid == item).val));
            }
            BinarySerializer.Serialize(filename,category);
            Ui.message("Export Ukończony");
        }

        public void ImportLesson()
        {
            var file = Ui.SelectFile("kategoria|*.category");
            if (!File.Exists(file))
            {
                return;
            }
            var category = (Category)BinarySerializer.Deserialize(file);
            manager.categories.Add(category.category);
            custom.SubcategoriesUniquePaths.Add(category.category.UniquePath);

            foreach (var item in category.questions)
            {
                manager.questions.Add(item.Item1);
                namer.list.Add((item.Item1.UniqueGuid, item.Item2));
                translations.AddQuestionDesc(item.Item1.UniqueGuid, item.Item1.Description);
            }
            var name = category.category.UniquePath.Split('\\').Last();
            translations.AddLessonDesc(name);
            Ui.message("Dodano lekcję");
        }

        public void RemoveLesson(QuestionCategory category)
        {
            foreach (var item in category.SubQuestionsGuids)
            {
                manager.questions.Remove(manager.questions.Find(s => s.UniqueGuid == item));
                namer.list.Remove(namer.list.Find(s => s.guid == item));
                translations.RemoveQuestionDesc(item);
            }
            manager.categories.Remove(category);
            custom.SubcategoriesUniquePaths.Remove($"\\custom\\{category.CategoryUserFriendlyName}");
            translations.RemoveLessonDesc(category.CategoryUserFriendlyName);
        }

        public void EditQuestion(Question question)
        {
            var questionToLoad = LoadQuestion();
            if (questionToLoad is null)
            {
                return;
            }          

            manager.questions.Remove(question);
            questionToLoad.UniqueGuid = question.UniqueGuid;
            questionToLoad.UserFriendlyName = question.UserFriendlyName;

            manager.questions.Add(questionToLoad);

            translations.RemoveQuestionDesc(question.UniqueGuid);
            translations.AddQuestionDesc(question.UniqueGuid, questionToLoad.Description);

            Ui.message("Edytowano pytanie");
        }

        public void RemoveQuestion(Question question,QuestionCategory category)
        {
                category.SubQuestionsGuids.Remove(question.UniqueGuid);
                manager.questions.Remove(question);
                namer.list.Remove(namer.list.Find(s => s.guid == question.UniqueGuid));
                translations.RemoveQuestionDesc(question.UniqueGuid);
            

        }        
    }
}
