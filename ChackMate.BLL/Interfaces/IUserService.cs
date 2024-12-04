using CheckMate.Domain.Models;

namespace ChackMate.BLL.Interfaces
{
    public interface IUserService
    {
        public User? Create(User user);
    }
}
