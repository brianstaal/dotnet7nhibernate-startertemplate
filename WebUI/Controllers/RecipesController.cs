using Domain.Persistence.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

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
            var recipes = await _recipeRepository.GetRecipesAsync();
            if (!recipes.Any())
            {
                return await Task.FromResult(NoContent());
            }

            var response = recipes.Select(recipe => new RecipeResponse
            {
                Id = recipe.Id,
                Name = recipe.Name,
                CategoryId = recipe.CategoryId,
                CategoryName = recipe.Category?.Name ?? string.Empty,
                Description = recipe.Description ?? string.Empty,
                Votes = recipe.Votes,
                DateCreate = recipe.DateCreate,
                DateChange = recipe.DateChange,
                Ingredients = (recipe.RecipeIngredients ?? Array.Empty<Domain.Entities.RecipeIngredient>())
                    .Select(ingredient => new RecipeIngredientResponse
                    {
                        IngredientId = ingredient.IngredientId,
                        IngredientName = ingredient.Ingredient?.Name ?? string.Empty,
                        AmountTypeId = ingredient.AmountTypeId,
                        AmountTypeName = ingredient.AmountType?.Name ?? string.Empty,
                        Amount = ingredient.Amount
                    })
                    .ToList()
            }).ToList();

            return Ok(response);
        }
    }
}
