using DevGuide.Models;
using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class SkillManager:BaseManager<Skill>
    {
        public SkillManager(ProjectContext context):base(context) { 

        }
        
    }
}
