using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ViewModels
{
    public class UserListViewModel
    {
        public string? ID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? Title { get; set; }

        public int? YearsOfExperience { get; set; }
        public string? Image { get; set; }

        public decimal? Price { get; set; }

        public decimal? AverageRate { get; set; }
        public string About { get; set; }
        public int? NumOfReviews { get; set; }

        public List<string> Skills { get; set; }
        public List<SocialLinkViewModel> SocialAccounts { get; set; }

    }
}
