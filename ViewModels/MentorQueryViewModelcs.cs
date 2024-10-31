using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class MentorQueryViewModelcs
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public string File { get; set; }
        public DateTime DateTime { get; set; }
        public string MentorName { get; set; }
        public string MentorImageUrl { get; set; }
        public string UserImageUrl { get; set; }
        public bool IsAnswered { get; set; }
    }
}
