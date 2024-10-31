using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserProfile
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? CVPath { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Level { get; set; }
        public string? Title { get; set; }
        public string? About { get; set; }
        public string? ImagePath { get; set; }
    }
}
