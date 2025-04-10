using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
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

        public IQueryable<Models.Ingredient> GetAllByListId(IEnumerable<long> ids)
        {
            return _dbSet.Where(i => ids.Contains(i.Id));
        }

        public IQueryable<Models.Ingredient> GetAllByFoodId(long foodId)
        {
            return _dbSet
                .Where(i => i.Foods.Any(f => f.Id == foodId)
            );
        }
    }
}