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
        public Task<Tournament>? Create(Tournament tournament, List<int> categoriesIds);
    }
}
