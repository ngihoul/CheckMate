using CheckMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Interfaces
{
    public interface ITournamentCategoryRepository
    {
        public Task<List<TournamentCategory>> GetAll();
    }
}
