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
            var translations = new Translations();
            var namer = new QuestionNamer();
            var manager = new BinaryManager();
            var ui = new UIRef();
            var logic = new MainLogic(ui,translations,namer,manager);
                    
            Application.Run(new Main(logic));
        }
    }
}
