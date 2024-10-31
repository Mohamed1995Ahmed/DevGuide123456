using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SocialLinkViewModel
    {
        public SocialAccountsTypes SocialName { get; set; }

        public string? SocialLink { get; set; }
    }
}
