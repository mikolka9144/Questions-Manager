using PixBlocks.DataModels.Questions;
using Points_submit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question_MAnager2
{
    public interface IBinaryManager
    {
        List<QuestionCategory> categories { get; }
        List<Question> questions { get; }

        void Save();
    }

    public class BinaryManager : IBinaryManager
    {
        public List<Question> questions { get; }
        public List<QuestionCategory> categories { get; }

        public BinaryManager()
        {
            questions = (List<Question>)BinarySerializer
               .Deserialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions.bin");
            categories = (List<QuestionCategory>)BinarySerializer
               .Deserialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories.bin");
        }

        public void Save()
        {
            BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\categories.bin", categories);
            BinarySerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + "_Data\\questions.bin", questions);
        }
    }
}
