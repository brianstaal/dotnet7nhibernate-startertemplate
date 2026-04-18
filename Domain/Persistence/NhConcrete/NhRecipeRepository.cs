using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Persistence.Abstract;
using NHibernate;
using NHibernate.Linq;

namespace Domain.Persistence.NhConcrete
{
    public class NhRecipeRepository : NhRepository, IRecipeRepository
    {
        public NhRecipeRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public async Task<IReadOnlyCollection<Recipe>> GetRecipesAsync()
        {
            return await ReadAsync(async session =>
            {
                var result = await session.Query<Recipe>()
                    .Fetch(recipe => recipe.Category)
                    .FetchMany(recipe => recipe.RecipeIngredients)
                    .ToListAsync();

                var recipeIds = result
                    .Select(recipe => recipe.Id)
                    .Distinct()
                    .ToArray();

                if (recipeIds.Length > 0)
                {
                    await session.Query<RecipeIngredient>()
                        .Where(recipeIngredient => recipeIds.Contains(recipeIngredient.RecipeId))
                        .Fetch(recipeIngredient => recipeIngredient.Ingredient)
                        .Fetch(recipeIngredient => recipeIngredient.AmountType)
                        .ToListAsync();
                }

                return (IReadOnlyCollection<Recipe>)result
                    .Distinct()
                    .ToList();
            });
        }
    }
}
