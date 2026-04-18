using Domain.Entities;
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
                return NoContent();
            }

            return Ok(recipes.Select(MapRecipe).ToList());
        }

        [HttpGet("{id:int}", Name = "GetRecipe")]
        public async Task<IActionResult> Get(int id)
        {
            var recipe = await _recipeRepository.GetRecipeAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(MapRecipe(recipe));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecipeWriteRequest request)
        {
            if (!HasDistinctIngredients(request))
            {
                return BadRequest("Each ingredient can only be included once per recipe.");
            }

            var createdRecipe = await _recipeRepository.CreateRecipeAsync(MapRecipe(request));
            return CreatedAtRoute("GetRecipe", new { id = createdRecipe.Id }, MapRecipe(createdRecipe));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] RecipeWriteRequest request)
        {
            if (!HasDistinctIngredients(request))
            {
                return BadRequest("Each ingredient can only be included once per recipe.");
            }

            var updatedRecipe = await _recipeRepository.UpdateRecipeAsync(MapRecipe(request, id));
            if (updatedRecipe == null)
            {
                return NotFound();
            }

            return Ok(MapRecipe(updatedRecipe));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _recipeRepository.DeleteRecipeAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        private static bool HasDistinctIngredients(RecipeWriteRequest request)
        {
            return request.Ingredients
                .Select(ingredient => ingredient.IngredientId)
                .Distinct()
                .Count() == request.Ingredients.Count;
        }

        private static Recipe MapRecipe(RecipeWriteRequest request, int id = 0)
        {
            return new Recipe
            {
                Id = id,
                Name = request.Name,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Votes = request.Votes,
                RecipeIngredients = request.Ingredients
                    .Select(ingredient => new RecipeIngredient
                    {
                        IngredientId = ingredient.IngredientId,
                        AmountTypeId = ingredient.AmountTypeId,
                        Amount = ingredient.Amount
                    })
                    .ToList()
            };
        }

        private static RecipeResponse MapRecipe(Recipe recipe)
        {
            return new RecipeResponse
            {
                Id = recipe.Id,
                Name = recipe.Name,
                CategoryId = recipe.CategoryId,
                CategoryName = recipe.Category?.Name ?? string.Empty,
                Description = recipe.Description ?? string.Empty,
                Votes = recipe.Votes,
                DateCreate = recipe.DateCreate,
                DateChange = recipe.DateChange,
                Ingredients = (recipe.RecipeIngredients ?? Array.Empty<RecipeIngredient>())
                    .Select(ingredient => new RecipeIngredientResponse
                    {
                        IngredientId = ingredient.IngredientId,
                        IngredientName = ingredient.Ingredient?.Name ?? string.Empty,
                        AmountTypeId = ingredient.AmountTypeId,
                        AmountTypeName = ingredient.AmountType?.Name ?? string.Empty,
                        Amount = ingredient.Amount
                    })
                    .ToList()
            };
        }
    }
}
