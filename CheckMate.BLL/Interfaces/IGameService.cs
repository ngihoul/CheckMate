using CheckMate.Domain.Models;

namespace CheckMate.BLL.Interfaces
{
    public interface IGameService
    {
        public Task<Game> GetById(int id);
        public Task<bool> PatchScore(Game game);
    }
}
