using KarmasisDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmasisDomain.Services
{ 
    public class TicketService
    {
        readonly TicketRepository repository;
        public TicketService(TicketRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("TicketRepository");
            }
            this.repository = repository;
      
        }
        public void CreateTicket(Ticket ticket)
        {
             repository.CreateTicket(ticket);
        }
        public Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return repository.GetAllTicketsAsync();
        }
        public bool Login(User user)
        {
            return repository.Login(user);
        }
        public Task<bool> SolvedTicketAsync(int id, string userName,bool solved)
        {
            return repository.SolvedTicketAsync(id, userName,solved);
        }
        public Task<bool> DeleteTicketAsync(int id)
        {
            return repository.DeleteTicketAsync(id);
        }
        public Task<IEnumerable<ChartEntity>> TopProblemProductsAsync()
        {
            return repository.TopProblemProductsAsync();
        }
        public Task<IEnumerable<ChartEntity>> TopSolvedByAsync()
        {
            return repository.TopSolvedByAsync();
        }

        public bool CheckDb()
        {
            return repository.CheckDb();
        }
    }
}
