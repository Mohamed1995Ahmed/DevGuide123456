using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class QueryWithAnswersViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string UserId { get; set; }
        public string File { get; set; }
        public DateTime DateTime { get; set; }

        // Mentor (instructor) information
        public string MentorFirstName { get; set; }
        public string MentorLastName { get; set; }
        public string MentorTitle { get; set; }
        public string MentorImageUrl { get; set; }

        // User information (who asked the query)
        public string UserImageUrl { get; set; }

        

        // List of answers
        public List<AddAnswerViewModel> QueryAnswers { get; set; }
    }
}
