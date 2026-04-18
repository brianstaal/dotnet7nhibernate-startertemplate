using System;
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

        public Task<IReadOnlyCollection<Recipe>> GetRecipesAsync()
        {
            return ReadAsync(session => LoadRecipesAsync(session));
        }

        public Task<Recipe?> GetRecipeAsync(int recipeId)
        {
            return ReadAsync(session => LoadRecipeAsync(session, recipeId));
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            try
            {
                await BeginTransactionAsync();

                recipe.DateChange = null;
                recipe.RecipeIngredients ??= new List<RecipeIngredient>();

                await Session.SaveAsync(recipe);
                await Session.FlushAsync();

                foreach (var recipeIngredient in recipe.RecipeIngredients)
                {
                    recipeIngredient.RecipeId = recipe.Id;
                    recipeIngredient.Recipe = recipe;
                    recipeIngredient.DateChange = null;
                    await Session.SaveAsync(recipeIngredient);
                }

                await CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }

            return await GetRecipeAsync(recipe.Id)
                ?? throw new InvalidOperationException($"Recipe {recipe.Id} could not be reloaded after creation.");
        }

        public async Task<Recipe?> UpdateRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            try
            {
                await BeginTransactionAsync();

                var existingRecipe = await LoadRecipeAsync(Session, recipe.Id);
                if (existingRecipe == null)
                {
                    await RollbackAsync();
                    return null;
                }

                existingRecipe.Name = recipe.Name;
                existingRecipe.CategoryId = recipe.CategoryId;
                existingRecipe.Description = recipe.Description;
                existingRecipe.Votes = recipe.Votes;
                existingRecipe.DateChange = DateTime.UtcNow;

                await SyncIngredientsAsync(existingRecipe, recipe.RecipeIngredients ?? Array.Empty<RecipeIngredient>());

                await Session.FlushAsync();
                await CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }

            return await GetRecipeAsync(recipe.Id);
        }

        public async Task<bool> DeleteRecipeAsync(int recipeId)
        {
            try
            {
                await BeginTransactionAsync();

                var existingRecipe = await LoadRecipeAsync(Session, recipeId);
                if (existingRecipe == null)
                {
                    await RollbackAsync();
                    return false;
                }

                foreach (var recipeIngredient in existingRecipe.RecipeIngredients.ToList())
                {
                    await Session.DeleteAsync(recipeIngredient);
                }

                await Session.DeleteAsync(existingRecipe);
                await Session.FlushAsync();
                await CommitAsync();
                return true;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        private async Task SyncIngredientsAsync(Recipe existingRecipe, IEnumerable<RecipeIngredient> incomingIngredients)
        {
            var incomingByIngredientId = incomingIngredients.ToDictionary(x => x.IngredientId);
            var recipeIngredients = existingRecipe.RecipeIngredients ?? new List<RecipeIngredient>();
            existingRecipe.RecipeIngredients = recipeIngredients;
            var existingIngredients = recipeIngredients.ToList();

            foreach (var existingIngredient in existingIngredients)
            {
                if (!incomingByIngredientId.TryGetValue(existingIngredient.IngredientId, out var updatedIngredient))
                {
                    recipeIngredients.Remove(existingIngredient);
                    await Session.DeleteAsync(existingIngredient);
                    continue;
                }

                existingIngredient.Amount = updatedIngredient.Amount;
                existingIngredient.AmountTypeId = updatedIngredient.AmountTypeId;
                existingIngredient.DateChange = DateTime.UtcNow;
            }

            var existingIngredientIds = new HashSet<int>(existingIngredients.Select(x => x.IngredientId));

            foreach (var incomingIngredient in incomingIngredients)
            {
                if (existingIngredientIds.Contains(incomingIngredient.IngredientId))
                    continue;

                var newIngredient = new RecipeIngredient
                {
                    RecipeId = existingRecipe.Id,
                    Recipe = existingRecipe,
                    IngredientId = incomingIngredient.IngredientId,
                    AmountTypeId = incomingIngredient.AmountTypeId,
                    Amount = incomingIngredient.Amount,
                    DateChange = null
                };

                recipeIngredients.Add(newIngredient);
                await Session.SaveAsync(newIngredient);
            }
        }

        private async Task<Recipe?> LoadRecipeAsync(ISession session, int recipeId)
        {
            var recipes = await LoadRecipesAsync(session, new[] { recipeId });
            return recipes.SingleOrDefault();
        }

        private async Task<IReadOnlyCollection<Recipe>> LoadRecipesAsync(ISession session, int[]? recipeIds = null)
        {
            IQueryable<Recipe> query = session.Query<Recipe>();
            if (recipeIds != null && recipeIds.Length > 0)
            {
                query = query.Where(recipe => recipeIds.Contains(recipe.Id));
            }

            var recipes = await query
                .Fetch(recipe => recipe.Category)
                .FetchMany(recipe => recipe.RecipeIngredients)
                .ToListAsync();

            var distinctRecipeIds = recipes
                .Select(recipe => recipe.Id)
                .Distinct()
                .ToArray();

            if (distinctRecipeIds.Length > 0)
            {
                await session.Query<RecipeIngredient>()
                    .Where(recipeIngredient => distinctRecipeIds.Contains(recipeIngredient.RecipeId))
                    .Fetch(recipeIngredient => recipeIngredient.Ingredient)
                    .Fetch(recipeIngredient => recipeIngredient.AmountType)
                    .ToListAsync();
            }

            return recipes
                .Distinct()
                .ToList();
        }
    }
}
