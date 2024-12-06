using CheckMate.Domain.Models;

namespace CheckMate.BLL.Interfaces
{
    public interface ITournamentService
    {
        public Task<Tournament>? GetById(int id);
        public Task<List<Tournament>> GetLast(TournamentFilters? filters);
        public Task<Tournament>? Create(Tournament tournament, List<int> categoriesIds);
        public Task<bool> Delete(int id);
        public Task<bool> Register(int tournamentId, int userId);
        public Task<TournamentPlayerStatus> GetRegisterInfo(Tournament tournament, int userId);
    }
}
