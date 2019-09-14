using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmasisDomain.Repositories
{
    public interface TicketRepository
    {
        bool Login(User user);
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<bool> SolvedTicketAsync(int id, string username,bool solved);
        Task<bool> DeleteTicketAsync(int id);
        Task<IEnumerable<ChartEntity>> TopProblemProductsAsync();
        void CreateTicket(Ticket ticket);
        Task<IEnumerable<ChartEntity>> TopSolvedByAsync();
        bool CheckDb();
    }
}
