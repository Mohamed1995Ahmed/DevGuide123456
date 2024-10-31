using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SubmitQuizViewModel
    {
        public int UserQuizId { get; set; }  // The User's quiz session ID
        public List<AnswerViewModel> Answers { get; set; }
    }
}
