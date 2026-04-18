using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Persistence.Abstract;
using NHibernate;

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
                var result = await session.QueryOver<Recipe>()
                    .ListAsync<Recipe>();

                return (IReadOnlyCollection<Recipe>)result
                    .Distinct()
                    .ToList();
            });
        }
    }
}
