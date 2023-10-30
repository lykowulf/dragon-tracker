using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestTracker2020.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }//gets and sets first name of user
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }//gets and sets last name of user 

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [Display(Name = "Avatar")]
        public string ImagePath { get; set; }
        public byte[] ImageData { get; set; }

        public List<ProjectUser> ProjestUsers { get; set; }
    }
}
