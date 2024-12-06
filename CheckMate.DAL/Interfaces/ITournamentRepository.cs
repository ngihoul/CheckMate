using CheckMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Interfaces
{
    public interface ITournamentRepository
    {
        public Task<Tournament>? GetById(int id);
        public Task<List<Tournament>> GetLast(TournamentFilters filters);
        public Task<Tournament>? Create(Tournament tournament);
        
        public Task<bool> Delete(Tournament tournament); 
        public Task<bool> Register(Tournament tournament, User user);
        public Task<bool> IsRegistered(Tournament tournament, User user);
        public Task<int> GetAttendees(Tournament tournament);
    }
}
