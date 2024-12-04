using CheckMate.Domain.Models;

namespace CheckMate.DAL.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role?> GetByName(string name);
    }
}
