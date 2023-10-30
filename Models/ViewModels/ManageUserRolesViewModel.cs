using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTracker2020.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public BTUser User { get; set; }//store details of the user whose roles are being managed
        public SelectList Roles { get; set; }//list of roles that can be assigned to the user. populates a drop down list in the view
        public string[] SelectedRoles { get; set; }//roles selected by a user and assigned to another




    }
}
