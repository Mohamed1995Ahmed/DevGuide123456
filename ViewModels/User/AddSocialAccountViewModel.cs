using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddSocialAccountViewModel
    {
        public SocialAccountsTypes SocialName { get; set; } // Enum type for social media platform
        public string SocialLink { get; set; } // URL for the social media profile
    }

}
