using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace ViewModels
{
    public class CreateQuizDto
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int NumberOfQuestions { get; set; }
        public int Duration { get; set; } // in minutes
        public List<QuestionDto> Questions { get; set; }
    }
}
