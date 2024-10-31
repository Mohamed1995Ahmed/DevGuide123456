using DevGuide.Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class ExperienceManager:BaseManager<Experience>
    {
        public ExperienceManager(ProjectContext context) : base(context)
        {

        }
    }
}
