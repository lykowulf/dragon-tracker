using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTracker2020.Models;

namespace TestTracker2020.Services
{
    public interface IBTHistortyService

       

    {
        Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId);
    }
}
