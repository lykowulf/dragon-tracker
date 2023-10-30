using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestTracker2020.Data;
using TestTracker2020.Models;

namespace TestTracker2020.Controllers
{
    
       
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;

        }

        //general access to the applications

        public IActionResult LandingPage()
        {
            return View();
        }


        //my dashboard for after login
        public IActionResult Index()
        {
            var model = new List<Ticket>();
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Admin"))
            {
                model = _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .ToList();
            }
            else if (User.IsInRole("ProjectManager"))
            {
                var projectIds = new List<int>();

                var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();

                foreach (var record in userProjects)
                {
                    projectIds.Add(record.ProjectId);
                }
                foreach (var id in projectIds)
                {
                    var tickets = _context.Tickets.Where(t => t.ProjectId == id)
                        .Include(t => t.DeveloperUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType)
                        .ToList();
                    model.AddRange(tickets);
                }
            }
            else if (User.IsInRole("Developer"))
            {
                model = _context.Tickets.Where(t => t.DeveloperUserId == userId)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .ToList();
            }
            else if (User.IsInRole("Submitter"))
            {
                model = _context.Tickets.Where(t => t.OwnerUserId == userId)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .ToList();
            }
            else if (User.IsInRole("NewUser"))
            {
                model = _context.Tickets
                    .Where(t => t.OwnerUserId == userId)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)



                   .Include(t => t.TicketPriority)
                   .Include(t => t.TicketStatus)
                   .Include(t => t.TicketType).ToList();
            }
            else
            {
                return View(model);
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
