using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTracker2020.Data;
using TestTracker2020.Models;
using TestTracker2020.Services;

namespace TestTracker2020.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTHistortyService _historyService;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTAccessService _accessService;

        public TicketsController(ApplicationDbContext context, IBTHistortyService histortyService, UserManager<BTUser> userManager, IBTAccessService accessService)
        {
            _context = context;
            _historyService = histortyService;
            _userManager = userManager;
            _accessService = accessService;
        }



        // GET: Tickets
        public IActionResult MyTickets()
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
            else
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.DeveloperUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var roleName = (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).FirstOrDefault();
            //if (await _accessService.CanInteractTicket(userId, (int)id, roleName))
            //{

                var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments)
                .Include(t => t.Attachments)
                .ThenInclude(tc => tc.User)
                .Include(th => th.Histories)
                .ThenInclude(th => th.User)
                .FirstOrDefaultAsync(m => m.Id == id);
                if (ticket == null)
                {
                    return NotFound();
                }



                //TempData["InvalidAccess"] = "You have attempted to access "
                //return RedirectToAction("Index");
            //}
                return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        public IActionResult Create()
        {
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ticket.TicketPriorityId == 0)
                    {
                        ticket.TicketPriorityId = _context.TicketPriorities.Where(tp => tp.Name == "Low").FirstOrDefault().Id;

                    }
                    if (ticket.TicketStatusId == 0)
                    {

                        ticket.TicketStatusId = _context.TicketStatuses.Where(tp => tp.Name == "Unassigned").FirstOrDefault().Id;
                    }

                    ticket.Created = DateTimeOffset.Now;
                    ticket.OwnerUserId = _userManager.GetUserId(User);

                    var priority = _context.TicketPriorities.FirstOrDefaultAsync(t => t.Name == "Low");

                    if (priority! == null)
                    {


                        ticket.TicketPriorityId = priority.Id;
                    }

                    var status = _context.TicketStatuses.FirstOrDefaultAsync(t => t.Name == "New");

                    if (status! == null)
                    {
                        ticket.TicketStatusId = status.Id;

                    }

                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                }
                catch { throw; }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var roleName = (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).FirstOrDefault();
            if (await _accessService.CanInteractTicket(userId, (int)id, roleName))
            {



                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            return RedirectToAction("Index");
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //send new data to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {
            //this goes to all places where SaveChangesAsync is located in posts  
            if (!User.IsInRole("Demo"))
            {
                if (id != ticket.Id)
                {
                    return NotFound();
                }

                //old ticket
                Ticket oldTicket = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                if (ModelState.IsValid)
                {
                    try
                    {
                        ticket.Updated = DateTimeOffset.Now;
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketExists(ticket.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    //add history for new ticket
                    string userId = _userManager.GetUserId(User);
                    Ticket newTicket = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);
                    await _historyService.AddHistory(oldTicket, newTicket, userId);

                    return RedirectToAction(nameof(Index));
                }


            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Name", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Name", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
            }
                else
                {
                    TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the databse please log in as a full user.";
                    return RedirectToAction("Index", "Tickets", new { id = ticket.ProjectId });
                }
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
