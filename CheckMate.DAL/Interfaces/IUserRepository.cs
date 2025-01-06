using CheckMate.Domain.Models;

namespace CheckMate.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> Create(User user);
        public Task<User?> GetByEmail(string email);
        public Task<User?> GetByEmailForLogin(string email);
        public Task<User?> GetByUsername(string username);
        public Task<User?> GetByUsernameForLogin(string username);
        public Task<User?> GetById(int id);
        public Task<List<User>> GetByCategories(IEnumerable<TournamentCategory> categories);
        public Task<List<User>> GetByTournament(int tournamentId);
        public Task<User?> Patch(User user);
    }
}
