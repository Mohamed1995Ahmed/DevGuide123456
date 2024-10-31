using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SkillUpdateViewModel
    {
        public int Id { get; set; } // The ID of the skill to update
        public string Name { get; set; } // The name of the skill to update
        public string? Description { get; set; } // New description for the skill
    }
}
