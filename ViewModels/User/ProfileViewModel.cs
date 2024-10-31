using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ProfileViewModel
    {
        public string? ID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? Title { get; set; }

        public int? YearsOfExperience { get; set; }
        public int? NumOfReviews { get; set; }
        public int? NumOfStudents { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }
        public string? Country { get; set; }
        public string? Level { get; set; }
        public decimal? AverageRate { get; set; }
        public string? About { get; set; }
        public List<string> Skills { get; set; }
        public List<SocialLinkViewModel>? SocialAccounts { get; set; }
        public List<EducationViewModel>? Educations { get; set; }
        public List<ExperienceViewModel>? Experiences { get; set; }

        //public List<ReviewViewModel>? Reviews { get; set; }



    }
}
