using TestTracker2020.Data;
using TestTracker2020.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestTracker2020.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        
        public BTProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region IsUserOnProject
        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);

            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;
        }
        #endregion

        #region ListUserProject
        public async Task<ICollection<Models.Project>> ListUserProjects(string userId)
        {
            BTUser user = await _context.Users
                .Include(p => p.ProjestUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);
            List<Project> projects = user.ProjestUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
            return projects;
        }
        #endregion

        #region AddUserToProject
        public async Task AddUserToProject(string userId, int projectId)
        {
            if(!await IsUserOnProject(userId, projectId))
            {
                try
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUser { ProjectId = projectId, UserId = userId });
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"********Error********* - Error Adding user to project. --> {ex.Message}");
                    throw;
                }
            }
        }
        #endregion

        #region RemoveUserFromProject
        public async Task RemoveUserFromProject(string userId, int projectId)
        {
           if(await IsUserOnProject(userId, projectId))
            {
                try
                {
                    //ProjectUser projectUser = new ProjectUser()
                    //{
                    //    ProjectId = projectId,
                    //    UserId = userId
                    //};
                    //_context.ProjectUsers.Remove(projectUser);
                    //await _context.SaveChangesAsync();
                    ProjectUser projectUser = await _context.ProjectUsers
                        .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                        .FirstOrDefaultAsync();

                    _context.ProjectUsers.Remove(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("***An error Occurred When Removing the User from the Project***");
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        #endregion


        #region UsersOnProject
        public async Task<ICollection<BTUser>> UsersOnProject(int projectId)
        {
            Project project = await _context.Projects
                 .Include(u => u.ProjectUsers).ThenInclude(u => u.User)
                 .FirstOrDefaultAsync(u => u.Id == projectId);

            List<BTUser> projectUsers = project.ProjectUsers.Select(p => p.User).ToList();
            return projectUsers;
        }
        #endregion

       
        
        public async Task<ICollection<BTUser>> UsersNotOnProject(int projectId)
        {
            
            return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId).Result == false).ToListAsync();
        }

       
    }
}
