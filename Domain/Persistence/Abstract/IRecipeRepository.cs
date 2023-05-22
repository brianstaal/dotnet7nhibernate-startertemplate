using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Persistence.Abstract
{
    public interface IRecipeRepository : IRepository
    {
        Task<ICollection<Recipe>> GetRecipies();
    }
}
