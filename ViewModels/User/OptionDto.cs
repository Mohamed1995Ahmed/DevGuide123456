using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }      // Whether this option is correct

    }
}
