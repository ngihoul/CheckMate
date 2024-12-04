using CheckMate.Domain.Models;

namespace ChackMate.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> Create(User user);
        public Task<User?> CreateByAdmin(User user);
        public Task<User?> ChooseUsername(int id, User user);
    }
}
