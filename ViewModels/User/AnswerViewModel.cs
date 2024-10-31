using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AnswerViewModel
    {
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }  // The option chosen by the user
    }
}
