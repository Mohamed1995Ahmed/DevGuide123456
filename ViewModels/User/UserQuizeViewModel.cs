using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserQuizeViewModel
    {

        public int Quiz_Id { get; set; }

        public List<UserQuizAnswerViewModel> MyAnswers { get; set;}
    }
    public class UserQuizAnswerViewModel
    {
        public int Option_Id { get; set; }
        public int Question_Id { get; set; }
    }
}
