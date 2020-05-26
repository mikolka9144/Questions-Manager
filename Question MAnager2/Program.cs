using PixBlocks.DataModels.Questions;
using Points_submit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Question_MAnager2
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            List<Question> questions = (List<Question>)BinarySerializer
                .Deserialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions.bin");
            List<QuestionCategory> categories = (List<QuestionCategory>)BinarySerializer
                .Deserialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories.bin");
            var translations = new Translations();
            var namer = new QuestionNamer();
            var logic = new MainLogic(new UIRef(namer), questions, categories, translations,namer);

            if (!categories.Any(s => s.CategoryUserFriendlyName == "custom"))
            {
                MessageBox.Show("TEst");
                var core = new QuestionCategory();
                core.CategoryUserFriendlyName = "custom";
                core.UniquePath = "\\custom";
                translations.LessonDesc.Add("\\custom\tPytania Niestandartowe\tCustom Tasks\tНестандартные вопросы");
                categories[0].SubcategoriesUniquePaths.Add("\\custom");
                categories.Add(core);
                logic.SaveAll();
            }          
            Application.Run(new Main(logic));
        }
    }
}
