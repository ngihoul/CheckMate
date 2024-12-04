using CheckMate.Domain.Models;

namespace CheckMate.DAL.Interfaces
{
    public interface IRoleRepository
    {
        public Role? GetByName(string name);
    }
}
