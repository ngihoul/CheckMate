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
        public Task<List<TournamentResult>> GetResult(Tournament tournament, int? round = 0);
        public Task<Tournament>? Create(Tournament tournament);
        public Task<Tournament?> Update(int id, Tournament tournament);
        public Task<bool> Delete(Tournament tournament); 
        public Task<bool> Register(Tournament tournament, User user);
        public Task<bool> Unregister(Tournament tournament, User user);
        public Task<bool> IsRegistered(Tournament tournament, User user);
        public Task<IEnumerable<User>> GetAttendees(Tournament tournament);
        public Task<int> GetNbAttendees(Tournament tournament);
        public Task<int> GetMaxRound(Tournament tournament);
    }
}
