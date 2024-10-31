using DevGuide.Models;
using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class SocialManager:BaseManager<SocialAccounts>
    {
        public SocialManager(ProjectContext context) : base(context)
        {

        }
    }
}
