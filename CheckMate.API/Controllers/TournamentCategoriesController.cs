using CheckMate.API.Mappers;
using CheckMate.BLL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentCategoriesController : ControllerBase
    {
        private readonly ITournamentCategoryService _categoryService;
        public TournamentCategoriesController(ITournamentCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentCategory>>> GetAll()
        {
            IEnumerable<TournamentCategory> categories = await _categoryService.GetAll();

            if (categories.Count() <= 0)
            {
                throw new ArgumentException("Aucune categorie trouvée");
            }

            return Ok(categories);
        }
    }
}
