using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestTracker2020.Models;

namespace TestTracker2020.Data
{
    public enum Roles
    {
        Admin,
        ProjectManager,
        Developer,
        Submitter,
        NewUser,
        Demo
    }




    public static class ContextSeed
    {
        public static async Task RunSeedMethodsAsync(RoleManager<IdentityRole> roleManager, UserManager<BTUser> userManager, ApplicationDbContext context)
        {
            await SeedRolesAsync(roleManager);
            await SeedDefaultUsersAsync(userManager);
            await SeedDefaultTicketTypeAysnc(context);
            await SeedDefaultTicketStatusAysnc(context);
            await SeedDefaultTicketPriorityAysnc(context);
            await SeedProjectsAsync(context);
            await SeedProjectUsersAsync(context, userManager);
            await SeedTicketsAsync(context, userManager);
        }




        //seed roles
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Demo.ToString()));


        }

        public static async Task SeedDefaultUsersAsync(UserManager<BTUser> userManager)
        {
            var defaultUser = new BTUser
            {
                UserName = "ash_ketchup65@mailinator.com",
                Email = "ash_ketchup65@mailinator.com",
                FirstName = "Joe",
                LastName = "Smith",
                EmailConfirmed = true
            };

            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }

            #region ProjectManager


            defaultUser = new BTUser
            {
                UserName = "pikachu235@mailinator.com",
                Email = "pikachu235@mailinator.com",
                FirstName = "Bobby",
                LastName = "Price",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project Manager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            #region Developer


            defaultUser = new BTUser
            {
                UserName = "squrtle235@mailinator.com",
                Email = "squrtle235@mailinator.com",
                FirstName = "Jane",
                LastName = "Doe",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            #region Submitter

            defaultUser = new BTUser
            {
                UserName = "charmandar235@mailinator.com",
                Email = "charmandar235@mailinator.com",
                FirstName = "char",
                LastName = "mandar",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            #region NewUser

            defaultUser = new BTUser
            {
                UserName = "bulbasaur235@mailinator.com",
                Email = "bulbasaur235@mailinator.com",
                FirstName = "Jon",
                LastName = "Smith",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            //these are my seeded demo users for showing off the software
            //each user occupies a "main" role and the new demo role
            //we will target this demo role to prevent demo users from changing the database

            #region Demo Admin Seed
            defaultUser = new BTUser
            {
                UserName = "demojohto_ash_ketchup65@gmail.com",
                Email = "demojohto_ash_ketchup65@gmail.com",
                FirstName = "Ashley",
                LastName = "Hannah",
                EmailConfirmed = true
            };

            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "My%Name%Jeff235!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Demo Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }
            #endregion

            #region Demo ProjectManager


            defaultUser = new BTUser
            {
                UserName = "demoraichu235@gmail.com",
                Email = "demoraichu235@gmail.com",
                FirstName = "Jason",
                LastName = "chu",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "My%Name%Jeff235!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Demo Project Manager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            #region Demo Developer


            defaultUser = new BTUser
            {
                UserName = "demowarturtle235@gmail.com",
                Email = "demowarturtle235@gmail.com",
                FirstName = "Chuck",
                LastName = "Norris",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "My%Name%Jeff235!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Demo Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            #region Demo Submitter

            defaultUser = new BTUser
            {
                UserName = "democharmileon235@gmail.com",
                Email = "democharmileon235@gmail.com",
                FirstName = "chow",
                LastName = "Lyu",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "My%Name%Jeff235!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Demo Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion

            #region Demo NewUser

            defaultUser = new BTUser
            {
                UserName = "demoivysaur235@gmail.com",
                Email = "demoivysaur235@gmail.com",
                FirstName = "Bucky",
                LastName = "Larsen",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "My%Name%Jeff235!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Demo New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            }


            #endregion
        }

        public static async Task SeedDefaultTicketTypeAysnc(ApplicationDbContext context)
        {
            var defaultSeedUI = new TicketType
            {
                Name = "UI"
            };
            try
            {
                var ticketType = await context.TicketTypes.Where(tt => tt.Name == "UI").FirstOrDefaultAsync();



                if (ticketType == null)
                {
                    context.TicketTypes.Add(defaultSeedUI);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********** ERROR **********");
                Debug.WriteLine("Error Seeding Default UI Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***************************");
            }

            defaultSeedUI = new TicketType
            {
                Name = "Open"
            };
            try
            {
                var ticketType = await context.TicketTypes.Where(tt => tt.Name == "Open").FirstOrDefaultAsync();



                if (ticketType == null)
                {
                    context.TicketTypes.Add(defaultSeedUI);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Open Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***************************");
            }

            defaultSeedUI = new TicketType
            {
                Name = "Urgent"
            };
            try
            {
                var ticketType = await context.TicketTypes.Where(tt => tt.Name == "Urgent").FirstOrDefaultAsync();



                if (ticketType == null)
                {
                    context.TicketTypes.Add(defaultSeedUI);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Urgent Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***************************");
            }

            defaultSeedUI = new TicketType
            {
                Name = "Resolved"
            };
            try
            {
                var ticketType = await context.TicketTypes.Where(tt => tt.Name == "Resolved").FirstOrDefaultAsync();



                if (ticketType == null)
                {
                    context.TicketTypes.Add(defaultSeedUI);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Resolved Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***************************");
            }
        }

        public static async Task SeedDefaultTicketStatusAysnc(ApplicationDbContext context)
        {
            try
            {
                if (!context.TicketStatuses.Any(tp => tp.Name == "New"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "New" });
                }
                if (!context.TicketStatuses.Any(tp => tp.Name == "In Progress"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "In Progress" });
                }
                if (!context.TicketStatuses.Any(tp => tp.Name == "Finished"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Finished" });
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error***");
                Debug.WriteLine("Error Seeding Ticket Status.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("******");
                throw;
            }
        }

        public static async Task SeedDefaultTicketPriorityAysnc(ApplicationDbContext context)
        {

            try
            {
                if (!context.TicketPriorities.Any(tp => tp.Name == "Low"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Low" });
                }
                if (!context.TicketPriorities.Any(tp => tp.Name == "Mid"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Mid" });
                }
                if (!context.TicketPriorities.Any(tp => tp.Name == "High"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "High" });
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error***");
                Debug.WriteLine("Error Seeding Ticket Priorites");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("******");
                throw;
            }
        }

        public static async Task SeedProjectsAsync(ApplicationDbContext context)
        {


            Project seedProject1 = new Project
            {
                Name = "Blog Project"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog Project");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject1);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error***");
                Debug.WriteLine("***error seeding default project 1***");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("******");

            }

            Project seedProject2 = new Project
            {
                Name = "Bug Tracker Project"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker Project");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject2);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error***");
                Debug.WriteLine("***error seeding default project 2***");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("******");

            }

            Project seedProject3 = new Project
            {
                Name = "Financial Portal Project"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal Project");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject3);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error***");
                Debug.WriteLine("***error seeding default project 3***");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("******");

            }

        }

        public static async Task SeedProjectUsersAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("ash_ketchup65@mailinator.com")).Id;
            string projectManagerId = (await userManager.FindByEmailAsync("pikachu235@mailinator.com")).Id;
            string developerId = (await userManager.FindByEmailAsync("demowarturtle235@gmail.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("democharmileon235@gmail.com")).Id;
            
            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog Project")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker Project")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal Project")).Id;

            #region Admin
            ProjectUser projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            #endregion
            #region ProjectManager
            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };



            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            #endregion



            #region Developer
            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };



            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };



            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            #endregion



            #region Submitter
            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };



            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };



            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
            #endregion



        }

        public static async Task SeedTicketsAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string developerId = (await userManager.FindByEmailAsync("demowarturtle235@gmail.com"))?.Id;
            string submitterId = (await userManager.FindByEmailAsync("democharmileon235@gmail.com"))?.Id;

            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog Project"))?.Id ?? 0;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker Project"))?.Id ?? 0;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal Project"))?.Id ?? 0;



            int statusId = (await context.TicketStatuses.FirstOrDefaultAsync(ts => ts.Name == "New")).Id;
            int typeId = (await context.TicketTypes.FirstOrDefaultAsync(tt => tt.Name == "UI")).Id;
            int proirityId = (await context.TicketPriorities.FirstOrDefaultAsync(tp => tp.Name == "Low"))?.Id??0;



            
            Ticket ticket = new Ticket
            {
                Title = "Need more blog posts",
                Description = "It's not a real blog when you only have a single post. Our users have requested you present more content.",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project1Id,
                TicketPriorityId = proirityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId



            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Need more blog posts");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 1");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
           



          
            //New Ticket Create
            string aminId = (await userManager.FindByEmailAsync("jontwin77@yahoo.com"))?.Id;
            string projectManagerId = (await userManager.FindByEmailAsync("Nathan77@mailinator.com"))?.Id;




            ticket = new Ticket
            {

                Title = "Seeded Test 2",
                Description = "This is only a test 2.",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project2Id,
                TicketPriorityId = proirityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = aminId,
                OwnerUserId = projectManagerId



            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Seeded Test 2");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 2");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
           



         
            




            ticket = new Ticket
            {
                Title = "Test 3",
                Description = "This is only a test 3.",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project3Id,
                TicketPriorityId = proirityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId



            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Test 3");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("********* ERROR **********");
                Debug.WriteLine("Error Seeding Default Project 3");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************");
            };
        }
    }
}

