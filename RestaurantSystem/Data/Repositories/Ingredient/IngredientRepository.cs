using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;
using RestaurantSystem.DTOs;

namespace RestaurantSystem.Data.Repositories.Ingredient
{
    public class IngredientRepository : Repository<Models.Ingredient>, IIngredientRepository
    {
        public IngredientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<IngredientSelectApresentationDTO>> GetAllForSelectAsync()
        {
            return await _dbSet.Select(i => new IngredientSelectApresentationDTO()
            {
                Id = i.Id,
                Description = $"{i.Name} - {i.Price}"
            }).ToListAsync();
        }

        public async Task<ICollection<Models.Ingredient>> GetIngredientsByListIdAsync(IEnumerable<long> ids)
        {
            return await _dbSet.Where(i => ids.Contains(i.Id)).ToListAsync();
        }
    }
}
