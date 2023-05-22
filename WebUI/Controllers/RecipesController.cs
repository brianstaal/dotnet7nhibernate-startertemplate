using Domain.Persistence.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ILogger<RecipesController> _logger;
        private readonly IRecipeRepository _recipeRepository;
        
        public RecipesController(ILogger<RecipesController> logger, IRecipeRepository recipeRepository)
        {
            _logger = logger;
            _recipeRepository = recipeRepository;
        }

        [HttpGet(Name = "GetRecipes")]
        public async Task<IActionResult> Get()
        {
            var recipes = await _recipeRepository.GetRecipies();
            if (!recipes.Any())
            {
                return await Task.FromResult(NoContent());
            }
            return Ok(recipes.ToList());
        }



    }
}
