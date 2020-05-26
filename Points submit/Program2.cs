using PixBlocks.DataModels.Questions;
using PixBlocks.Tools.StringCompressor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Points_submit
{
    internal class Program2
    {
        static string lesson;
        static Translations translations = new Translations();
        private static QuestionCategory customs;
        private static List<Question> questions = (List<Question>)BinarySerializer.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions.bin");
        private static List<QuestionCategory> categories = (List<QuestionCategory>)BinarySerializer.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories.bin");

        private static string GetID(QuestionCategory category)
        {
            Console.WriteLine("Wprowadź intentyfikator pytania, lub wpisz \"all\" aby zobaczyć wszystkie intentyfikatory ");
            var ID = Console.ReadLine();
            if (ID == "all")
            {
                Console.WriteLine("Oto wszystkie intentyfikatory:");
                foreach (var item in category.SubQuestionsGuids)
                {
                    Console.WriteLine($"{item}");
                }
            }
            return ID;
        }

        private static string GetLesson()
        {
            while (true)
            {
                Console.WriteLine("Podaj nazwę lekcji. wpisz all dla wszystkich lekcji.");
                var lesson = Console.ReadLine();
                if (lesson == "all")
                {
                    Console.WriteLine("Oto lista wszystkich lekcji:");
                    foreach (var item in customs.SubcategoriesUniquePaths)
                    {
                        Console.WriteLine(item.Split('\\').Last());
                    }
                    
                }
                else
                {
                    return lesson;
                }
            }
        }
        private static void Case1()
        {
            QuestionCategory category;
            Console.WriteLine("Przeciągnij plik z pytaniem i kliknij enter.");
            var questionToLoad = (Question)new XmlSerializer(typeof(Question)).Deserialize(File.OpenRead(Console.ReadLine()));
            lesson = GetLesson();
            category = categories.Find(s => s.CategoryUserFriendlyName == lesson);
            if (category is null)
            {
                category = new QuestionCategory();
                category.CategoryUserFriendlyName = lesson;
                category.UniquePath = $"\\custom\\{category.CategoryUserFriendlyName}";
                customs.SubcategoriesUniquePaths.Add($"\\custom\\{category.CategoryUserFriendlyName}");
                categories.Add(category);
                translations.AddLessonDesc(category.CategoryUserFriendlyName);
            }

            Guid guid = Guid.NewGuid();
            category.SubQuestionsGuids.Add(guid.ToString());
            questionToLoad.UniqueGuid = guid.ToString();

            questions.Add(questionToLoad);
            translations.AddQuestionDesc(guid.ToString(), questionToLoad.Description);
            Console.WriteLine($"Dodano pytanie. Intentyfikator: {guid}");
        }

        private static void Main(string[] args)
        {
            
            if (!categories.Any(s => s.CategoryUserFriendlyName == "custom"))
            {
                var core = new QuestionCategory();
                core.CategoryUserFriendlyName = "custom";
                core.UniquePath = "\\custom";
                translations.LessonDesc.Add("\\custom\tPytania Niestandartowe\tCustom Tasks\tНестандартные вопросы");
                categories[0].SubcategoriesUniquePaths.Add("\\custom");
                categories.Add(core);
            }
            customs = categories.Find(s => s.CategoryUserFriendlyName == "custom");
            while (true)
            {
                
                Console.WriteLine("Wybierz akcje:");
                Console.WriteLine("1. Dodaj Pytanie");
                Console.WriteLine("2. Usun pytanie");
                Console.WriteLine("3. Edytuj pytanie");
                Console.WriteLine("4. Exportuj pytanie");
                Console.WriteLine("5. Usuń katagorię");
                Console.WriteLine("6. Exportuj lekcję");
                Console.WriteLine("7. Inportuj lekcję");
                switch (Console.ReadLine())
                {
                    case "1":
                        Case1();
                        break;

                    case "2":
                        Case2();
                        break;
                    case "3":
                        Case3();
                        break;
                    case "4":
                        Case4();
                        break;
                    case "5":
                        Case5();
                        break;
                    case "6":
                        Case6();
                        break;
                    case "7":
                        Case7();
                        break;
                }

                BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories.bin", categories);
                BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions.bin", questions);
                translations.Save();
            }
        }

        private static void Case4()
        {
            var category = GetCategory();
            var ID = GetID(category);
            if (category.SubQuestionsGuids.Exists(s => s == ID))
            {
                new XmlSerializer(typeof(Question)).Serialize
                    (File.Create(AppDomain.CurrentDomain.BaseDirectory +$"{ID}.question")
                    ,questions.Find(s => s.UniqueGuid == ID));
                Console.WriteLine("Export Ukończony");
                return;
            }
            Console.WriteLine("Brak Pytania");
        }

        private static void Case6()
        {
            var category = new Category();
            category.category = GetCategory();
            category.questions = new List<Question>(5);
            foreach (var item in category.category.SubQuestionsGuids)
            {
                category.questions.Add(questions.Find(s => s.UniqueGuid == item));
            }
            BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + $"{category.category.CategoryUserFriendlyName}.category",category);
            Console.WriteLine("Export Ukończony");
        }

        private static void Case7()
        {
            Console.WriteLine("Przeciągnij plik kategori i wciśnij enter");
            var category = (Category)BinarySerializer.Deserialize(Console.ReadLine());
            categories.Add(category.category);
            customs.SubcategoriesUniquePaths.Add(category.category.UniquePath);
            questions.AddRange(category.questions);
            var name = category.category.UniquePath.Split('\\').Last();
            translations.AddLessonDesc(name);
            foreach (var item in category.questions)
            {
                translations.AddQuestionDesc(item.UniqueGuid, item.Description);
            }
        }

        private static void Case5()
        {
            var category = GetCategory();
            foreach (var item in category.SubQuestionsGuids)
            {
                questions.Remove(questions.Find(s => s.UniqueGuid == item));
                translations.RemoveQuestionDesc(item);
            }
            categories.Remove(category);
            customs.SubcategoriesUniquePaths.Remove($"\\custom\\{category.CategoryUserFriendlyName}");
            translations.RemoveLessonDesc(lesson);
            Console.WriteLine("Usunięto pytanie");
        }

        private static void Case3()
        {
            Console.WriteLine("Przeciągnij plik z pytaniem i kliknij enter.");
            var questionToLoad = (Question)new XmlSerializer(typeof(Question)).Deserialize(File.OpenRead(Console.ReadLine()));
            var category = GetCategory();
            var ID = GetID(category);
            if (category.SubQuestionsGuids.Exists(s => s == ID))
            {
                var oldQuestion = questions.Find(s => s.UniqueGuid == ID);
                questions.Remove(oldQuestion);
                questionToLoad.UniqueGuid = oldQuestion.UniqueGuid;
                questionToLoad.UserFriendlyName = oldQuestion.UserFriendlyName;
                questions.Add(questionToLoad);
                translations.RemoveQuestionDesc(oldQuestion.UniqueGuid);
                translations.AddQuestionDesc(oldQuestion.UniqueGuid, questionToLoad.Description);
                Console.WriteLine("Edytowano pytanie");
            }
            else
            {
                Console.WriteLine("Brak pytania");
            }
        }

        private static void Case2()
        {
            var category = GetCategory();
            var ID = GetID(category);
            if (category.SubQuestionsGuids.Exists(s => s == ID))
            {
                category.SubQuestionsGuids.Remove(ID);
                questions.Remove(questions.Find(s => s.UniqueGuid == ID));
                translations.RemoveQuestionDesc(ID);
                Console.WriteLine("Usunieto pytane");
            }
            else
            {
                Console.WriteLine("Brak pytania");
            }
        }

        static QuestionCategory GetCategory()
        {
            lesson = GetLesson();
            var category = categories.Find(s => s.CategoryUserFriendlyName == lesson);
            if (category is null)
            {
                Console.WriteLine("Brak lekcji");
                return GetCategory();
            }
            return category;
        }
    }
}
