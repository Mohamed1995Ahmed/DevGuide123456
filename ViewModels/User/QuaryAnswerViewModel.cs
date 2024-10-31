using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class QuaryAnswerViewModel
    {
        public int Id { get; set; }
        public string Instructor_Id { get; set; }
        public string User_Id { get; set; }
        public string Question { get; set; }
        public string File { get; set; }
        public string FilePath { get; set; }
        public DateTime DateTime { get; set; }
        public string MentorTitle { get; set; }
        public string MentorImageUrl { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserImageUrl { get; set; }
        public List<QueryAnswer> QueryAnswers { get; set; }

   
    }


}
