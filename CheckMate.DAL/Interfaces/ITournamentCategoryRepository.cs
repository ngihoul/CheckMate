using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Interfaces
{
    public interface ITournamentCategoryRepository
    {
        public Task<IEnumerable<TournamentCategory>> GetAll();
        // TODO : utiliser tournamentId
        public Task<IEnumerable<TournamentCategory>> GetByTournament(int tournamentId);
    }
}
