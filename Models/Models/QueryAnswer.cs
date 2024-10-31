using DevGuide.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DevGuide.Models.Models
{
    public class QueryAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Answer { get; set; }
        public string? File { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDeveloper { get; set; }

        // Relationships
        public virtual Query Query { get; set; }
        public int Query_Id { get; set; } // Foreign Key to Query
    }
}
