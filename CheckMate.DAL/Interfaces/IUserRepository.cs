using CheckMate.Domain.Models;

namespace CheckMate.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> Create(User user);
        public Task<User?> GetByEmail(string email);
        public Task<User?> GetByUsername(string username);
        public Task<User?> GetById(int id);
        public Task<User?> Patch(User user);
    }
}
