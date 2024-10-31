using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public List<OptionDto> Options { get; set; }
    }
}
