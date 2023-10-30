using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTracker2020.Models
{
    public class Project
    {
        public Project()
        {

            Tickets = new HashSet<Ticket>();

        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }


        [Display(Name = "Project Image")]
        public string ImagePath { get; set; }
        public byte[] ImageData { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
