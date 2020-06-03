using PixBlocks.DataModels.Questions;
using Question_MAnager2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Points_submit
{
    public interface IUIRef
    {
        string GetName(string cap);
        void message(string v);
        string SelectFile(string format,string cap = "Wybierz plik");
    }

    public class UIRef : IUIRef
    {
        public void message(string v)
        {
            MessageBox.Show(v);
        }

        public string SelectFile(string format,string cap = "Wybierz plik")
        {
            var file = new OpenFileDialog();
            file.Filter = format;
            file.Title = cap;
            file.ShowDialog();
            return file.FileName;
        }

        public string GetName(string cap)
        {
            var textSelector = new NameSelect(cap);
            textSelector.ShowDialog();
            return textSelector.name;
        }
    }
}