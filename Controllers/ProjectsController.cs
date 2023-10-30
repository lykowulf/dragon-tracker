using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using TestTracker2020.Data;
using TestTracker2020.Models;
using TestTracker2020.Models.ViewModels;
using TestTracker2020.Services;

namespace TestTracker2020.Controllers
{
    [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter, NewUser")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTProjectService _bTProjectService;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTRolesService _rolesService;
        private readonly IBTHistortyService _historyService;
        private readonly IEmailSender _emailSender;
        
        public ProjectsController(ApplicationDbContext context, IBTProjectService bTProjectService, UserManager<BTUser> userManager, IBTRolesService rolesService, IBTHistortyService historyService, IEmailSender emailSender)
        {
            _context = context;
            _bTProjectService = bTProjectService;
            _userManager = userManager;
            _rolesService = rolesService;
            _historyService = historyService;
            _emailSender = emailSender;
        }

        // GET: Projects
        public IActionResult MyProjects()
        {
            var pmodel = new List<Project>();
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Admin"))
            {
                pmodel = _context.Projects.Include(p => p.ProjectUsers).ToList();
            }
            else
            {
                var projectIds = new List<int>();
                var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();

                foreach (var record in userProjects)
                {
                    projectIds.Add(record.ProjectId);
                }
                foreach(var id in projectIds)
                {
                    var project = _context.Projects.Find(id);
                    pmodel.Add(project);
                }
            }
            return View(pmodel);

        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tickets).ThenInclude(t => t.TicketPriority)
                .Include(p => p.Tickets).ThenInclude(t => t.TicketStatus)
                .Include(p => p.Tickets).ThenInclude(t => t.TicketType) 
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            var tickets = await _context.Tickets
                .Where(t => t.ProjectId == id)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketType)
                .Include(t => t.DeveloperUser)
                .ToListAsync();
            
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        //assign the users for the project
        //[authorize(roles = "admin, projectmanager")]


        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpGet]
        public async Task<IActionResult> AssignUsers(int id)
        {
            var model = new ProjectUsersViewModel();
            var project = await _context.Projects
            .Include(p => p.ProjectUsers)
            .ThenInclude(p => p.User)
            .FirstAsync(p => p.Id == id);
            model.Project = project;
            List<BTUser> users = await _context.Users.ToListAsync();
          
          
            List<BTUser> members = (List<BTUser>)await _bTProjectService.UsersOnProject(id);
            model.Users = new SelectList(users, "Id", "FullName", members.Select(m => m.Id));
           
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AssignUsers(ProjectUsersViewModel model)
        {
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    var currentMemebers = await _context.Projects.Include(p => p.ProjectUsers).FirstOrDefaultAsync(p => p.Id == model.Project.Id);
                    List<string> memberIds = currentMemebers.ProjectUsers.Select(u => u.UserId).ToList();
                    memberIds.ForEach(async (i) => await _bTProjectService.RemoveUserFromProject(i, model.Project.Id));
                    foreach (string id in memberIds)
                    {
                        await _bTProjectService.RemoveUserFromProject(id, model.Project.Id);
                    }
                    if (model.SelectedUsers != null)
                    {
                        foreach (string id in model.SelectedUsers)
                        {
                            await _bTProjectService.AddUserToProject(id, model.Project.Id);
                            var user = _context.Users.Find(id);
                            //Send an email
                            string devEmail = user.Email;
                            string subject = "New Project Assignment";
                            string message = $"You have a new project: {model.Project.Name}";
                            await _emailSender.SendEmailAsync(devEmail, subject, message);
                        }
                    }
                    else
                    {
                    }
                    return RedirectToAction("Details", "Projects", new { id = model.Project.Id });//action / controller / object
                }
                ICollection<BTUser> users = await _rolesService.UsersInRole("Developer, Submitter");
                model.Users = new SelectList(users, "Id", "FullName", model.SelectedUsers);
                return View(model);
            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database please log in as a full user.";
                return RedirectToAction("Index", "Projects", new { id = model.SelectedUsers });
                //return View("Details", "Tickets");
            }
        }

    }
}
