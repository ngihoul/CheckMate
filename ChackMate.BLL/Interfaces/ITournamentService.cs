using CheckMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.BLL.Interfaces
{
    public interface ITournamentService
    {
        public Task<List<Tournament>> GetLast();
        public Task<Tournament>? Create(Tournament tournament, List<int> categoriesIds);
        public Task<bool> Delete(int id);
    }
}
