using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddAnswerQueryViewModel
    {
        [Required]
        public string Answer { get; set; }

        //public int id { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? File { get; set; }
        public DateTime DateTime { get; set; }

        public int Query_Id { get; set; } // Foreign Key to Query
    }
}
