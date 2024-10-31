using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class QuizResultViewModel
    {
        public decimal Score { get; set; }  // Percentage score
        public bool Passed { get; set; }    // Pass/Fail based on score
    }
}
