using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace ViewModels
{
    public class AddQueryViewModel
    {
        //public int Id { get; set; }

        [Required]
        public string Question { get; set; }

        public string? FilePath { get; set; } // Optionally store the file path or file name
        public IFormFile? File { get; set; } // Optionally store the file path or file name
        public DateTime DateTime { get; set; }


        // Relationships (if needed)
        //public User User { get; set; }
        public string User_Instructor_Id { get; set; } // Foreign Key to Instructor

        public string? User_Id { get; set; } // Foreign Key to User
    }
}
