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
using TestTracker2020.Data;
using TestTracker2020.Models;
using TestTracker2020.Models.ViewModels;
using TestTracker2020.Services;

namespace TestTracker2020.Controllers
{
    //[Authorize]
    //public class ProjectUsersController : Controller
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly IBTProjectService _rolesService;
    //    private readonly UserManager<BTUser> _userManager;
    //    private readonly IBTHistortyService _historyService;

    //    public ProjectUsersController(ApplicationDbContext context, IBTProjectService rolesService, UserManager<BTUser> userManager, IBTHistortyService historyService)
    //    {
    //        _context = context;
    //        _userManager = userManager;
    //        _rolesService = rolesService;
    //        _historyService = historyService;

    //    }

    //    [Authorize]
    //    public class ProjectsController : Controller
    //    {
    //        private readonly ApplicationDbContext _context;//calls the database when needed
    //        private readonly IBTProjectService _projectsService;
    //        private readonly UserManager<BTUser> _userManager;
    //        private readonly IBTRolesService _roleService;
    //        private readonly IBTHistortyService _historyService;
    //        private readonly IEmailSender _emailSender;
    //        public ProjectsController(ApplicationDbContext context, IBTProjectService projectsService, UserManager<BTUser> userManager, IBTRolesService rolesService, IBTHistortyService historyService, IEmailSender emailSender)//constuctor recevices the injection (APPDbCONtext)
    //        {
    //            _context = context;//reference to database
    //            _projectsService = projectsService;
    //            _userManager = userManager;
    //            _roleService = rolesService;
    //            _historyService = historyService;
    //            _emailSender = emailSender;
    //        }
            
    //        // GET: Projects
    //        public IActionResult MyProject()
    //        {
    //            var pmodel = new List<Project>();
    //            var userId = _userManager.GetUserId(User);
    //            if (User.IsInRole("Admin"))
    //            {
    //                pmodel = _context.Projects.Include(p => p.ProjectUsers).ToList();
    //            }
    //            else
    //            {
    //                var projectIds = new List<int>();

    //                var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();
    //                foreach (var record in userProjects)
    //                {
    //                    projectIds.Add(record.ProjectId);
    //                }
    //                foreach (var id in projectIds)
    //                {
    //                    var project = _context.Projects.Find(id);
    //                    pmodel.Add(project);
    //                }
    //            }
    //            return View("MyProjects", pmodel);
    //        }

    //        public async Task<IActionResult> Index()
    //        {
    //            return View(await _context.Projects.ToListAsync());
    //        }
    //        // GET: Projects/Details/5
    //        public async Task<IActionResult> Details(int? id)
    //        {
    //            if (id == null)
    //            {
    //                return NotFound();
    //            }
    //            var project = await _context.Projects
    //                .Include(m => m.Tickets)
    //                .Include(m => m.ProjectUsers)
    //                .ThenInclude(m => m.User)
    //                .FirstOrDefaultAsync(m => m.Id == id);
    //            if (project == null)
    //            {
    //                return NotFound();
    //            }
    //            return View(project);
    //        }
    //        [Authorize(Roles = "Admin, ProjectManager")]
    //        // GET: Projects/Create
    //        public IActionResult Create()
    //        {
    //            return View();
    //        }
    //        // POST: Projects/Create
    //        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //        [HttpPost]
    //        [ValidateAntiForgeryToken]
    //        public async Task<IActionResult> Create([Bind("Id,Name,FormFile,ImagePath,ImageData")] Project project)
    //        {
    //            if (!User.IsInRole("Demo"))
    //            {
    //                if (ModelState.IsValid)
    //                {
    //                    _context.Add(project);
    //                    await _context.SaveChangesAsync();
    //                    return RedirectToAction(nameof(Index));
    //                }
    //                return View(project);
    //            }
    //            else
    //            {
    //                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database please log in as a full user.";
    //                return RedirectToAction("Index", "Projects", new { id = project.Tickets });
    //                //return View("Details", "Tickets");
    //            }
    //        }
    //        // GET: Projects/Edit/5
    //        public async Task<IActionResult> Edit(int? id)
    //        {
    //            if (id == null)
    //            {
    //                return NotFound();
    //            }
    //            var project = await _context.Projects.FindAsync(id);
    //            if (project == null)
    //            {
    //                return NotFound();
    //            }
    //            return View(project);
    //        }
    //        // POST: Projects/Edit/5
    //        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //        [HttpPost]
    //        [ValidateAntiForgeryToken]
    //        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
    //        {
    //            if (!User.IsInRole("Demo"))
    //            {
    //                if (id != project.Id)
    //                {
    //                    return NotFound();
    //                }
    //                if (ModelState.IsValid)
    //                {
    //                    try
    //                    {
    //                        _context.Update(project);
    //                        await _context.SaveChangesAsync();
    //                    }
    //                    catch (DbUpdateConcurrencyException)
    //                    {
    //                        if (!ProjectExists(project.Id))
    //                        {
    //                            return NotFound();
    //                        }
    //                        else
    //                        {
    //                            throw;
    //                        }
    //                    }
    //                    return RedirectToAction(nameof(Index));
    //                }
    //                return View(project);
    //            }
    //            else
    //            {
    //                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database please log in as a full user.";
    //                return RedirectToAction("Index", "Projects", new { id = project.Tickets });
    //            }

    //        }
    //        // GET: Projects/Delete/5
    //        public async Task<IActionResult> Delete(int? id)
    //        {
    //            if (id == null)
    //            {
    //                return NotFound();
    //            }
    //            var project = await _context.Projects
    //                .FirstOrDefaultAsync(m => m.Id == id);
    //            if (project == null)
    //            {
    //                return NotFound();
    //            }
    //            return View(project);
    //        }
    //        // POST: Projects/Delete/5
    //        [HttpPost, ActionName("Delete")]
    //        [ValidateAntiForgeryToken]
    //        public async Task<IActionResult> DeleteConfirmed(int id)
    //        {
    //            if (!User.IsInRole("Demo"))
    //            {
    //                var project = await _context.Projects.FindAsync(id);
    //                _context.Projects.Remove(project);
    //                await _context.SaveChangesAsync();
    //                return RedirectToAction(nameof(Index));
    //            }
    //            else
    //            {
    //                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database please log in as a full user.";
    //                return RedirectToAction("Index", "Projects");
    //                //return View("Details", "Tickets");
    //            }
    //        }
    //        private bool ProjectExists(int id)
    //        {
    //            return _context.Projects.Any(e => e.Id == id);
    //        }
    //    }
    //}









    //    public async Task<IActionResult> ProjectUserRoles()
    //    {
    //        List<ProjectUsersViewModel> model = new List<ProjectUsersViewModel>();
    //        List<BTUser> project = _context.Projects.ToList();

    //        foreach (var user in project)
    //        {
    //            ProjectUsersViewModel vm = new ProjectUsersViewModel();
    //            vm.Project = user;
    //            var selected = await _rolesService.ListUserProjects(user);
    //            vm.Users = new MultiSelectList(_context.Roles, "Name", "Name", selected);
    //            model.Add(vm);
    //        }
    //        return View(model);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> ProjectUserRoles(ProjectUsersViewModel vmUser)
    //    {
    //        BTUser user = _context.Users.Find(vmUser.User.Id);

    //        IEnumerable<string> roles = await _rolesService.ListUserProjects(user);
    //        await _userManager.RemoveFromRolesAsync(user, roles);
    //        string userRole = vmUser.SelectedRoles.FirstOrDefault();

    //        if (Enum.TryParse(userRole, out Roles roleValue))
    //        {
    //            await _rolesService.AddUserToProject(user, userRole);
    //            return RedirectToAction("ManageUserRoles");

    //        }
    //        return RedirectToAction("ManageUserRoles");
    //    }
}


 