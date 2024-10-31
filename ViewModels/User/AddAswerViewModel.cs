using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddAnswerViewModel
    {
        public string Answer { get; set; }
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }
        public int Query_Id { get; set; } // Associate the answer with a query
        public DateTime DateTime { get; set; }
        public int Id { get; set; }
        public bool IsDeveloper { get; set; }
        public string User_Id { get; set; }
       
    }
}
