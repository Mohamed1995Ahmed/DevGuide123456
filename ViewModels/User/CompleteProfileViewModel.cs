using DevGuide.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CompleteProfileViewModel
    {
        public string? UserId { get; set; }
        public IFormFile? CV { get; set; }
        public string? CV_Path { get; set; } = "";
        public string? Country { get; set; }
        public string? PhoneNumber {  get; set; }
        public string? About { get; set; }
        public string? Title { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Level { get; set; }
        public IFormFile? Image { get; set; }
        public string? Image_Path { get; set; } = "";
        public  List<int>? Skills { get; set; }
    }
}
