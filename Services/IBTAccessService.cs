﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTracker2020.Services
{
    public interface IBTAccessService
    {
        public Task<bool> CanInteractProject(string userId, int projectId, string roleName);
        public Task<bool> CanInteractTicket(string userId, int ticketId, string roleName);

        





    }
}
