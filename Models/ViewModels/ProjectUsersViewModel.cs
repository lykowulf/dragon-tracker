using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTracker2020.Models.ViewModels
{
    public class ProjectUsersViewModel
    {
        public Project Project { get; set; } //store deatils of the project that the users will be associated with
        public MultiSelectList Users { get; set; }//populates list box within the view
        public string[] SelectedUsers{ get; set; }//receives selected users 
    }
}
