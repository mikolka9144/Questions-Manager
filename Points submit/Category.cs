using PixBlocks.DataModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points_submit
{
    [Serializable]
    public class Category
    {
        public QuestionCategory category { get; set; }
        public List<Question> questions { get; set; }
    }
}
