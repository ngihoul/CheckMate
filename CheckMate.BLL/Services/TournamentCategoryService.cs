using CheckMate.BLL.Interfaces;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;

namespace CheckMate.BLL.Services
{
    public class TournamentCategoryService : ITournamentCategoryService
    {
        private readonly ITournamentCategoryRepository _categoryRepository;

        public TournamentCategoryService(ITournamentCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<IEnumerable<TournamentCategory>> GetAll()
        {
            return _categoryRepository.GetAll();
        }
    }
}
