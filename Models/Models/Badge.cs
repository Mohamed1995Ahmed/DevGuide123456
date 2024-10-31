
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Badge
    {
        public int Id { get; set; }
        public string? BadgeType { get; set; }
        public virtual ICollection<User_Badges>? Badges { get; set; }

    }

   

}
