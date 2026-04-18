using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Persistence.Abstract
{
    public interface IRecipeRepository
    {
        Task<IReadOnlyCollection<Recipe>> GetRecipesAsync();
        Task<Recipe> CreateRecipeAsync(Recipe recipe);
        Task<bool> DeleteRecipeAsync(int recipeId);
        Task<Recipe?> GetRecipeAsync(int recipeId);
        Task<Recipe?> UpdateRecipeAsync(Recipe recipe);
    }
}
