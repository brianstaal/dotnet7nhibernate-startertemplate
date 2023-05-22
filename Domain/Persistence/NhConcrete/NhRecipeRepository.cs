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
        public NhRecipeRepository(ISession session, ISessionFactory sessionFactory) : base(session, sessionFactory)
        {
        }

        public async Task<ICollection<Recipe>> GetRecipies()
        {
            Recipe r = null;

            var query = Session.QueryOver(() => r);

            var result = await query
                .ListAsync<Recipe>();

            return result
                .Distinct()
                .ToList();
        }
    }
}
