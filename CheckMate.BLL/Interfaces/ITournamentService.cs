using CheckMate.Domain.Models;

namespace CheckMate.BLL.Interfaces
{
    public interface ITournamentService
    {
        public Task<Tournament>? GetById(int id);
        public Task<IEnumerable<Tournament>> GetLast(TournamentFilters? filters);
        public Task<IEnumerable<TournamentResult>> GetResult(int tournamentId, int? round = 0);
        public Task<Tournament>? Create(Tournament tournament, IEnumerable<int> categoriesIds);
        public Task<bool> Delete(int id);
        public Task<bool> Register(int tournamentId, int userId);
        public Task<bool> Unregister(int tournamentId, int userId);
        public Task<bool> Start(int tournamentId);
        public Task<Tournament> NextRound(int tournamentId);
        public Task<TournamentPlayerStatus> GetRegisterInfo(Tournament tournament, int userId);
    }
}
