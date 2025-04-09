
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories.Food
{
    public class FoodRepository : Repository<Models.Food>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<Models.Food?> GetByIdWithIngredientsAsync(long id)
        {
            return await _dbSet.Include(f => f.OptionalIngredients).SingleOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<CatalogViewModel>> GetCatalogAsync()
        {
            return await _dbSet.Select(f =>
                new CatalogViewModel()
                {
                    FoodId = f.Id,
                    FoodName = f.Name,
                    FoodDescription = f.Description,
                    FoodPrice = f.BasePrice
                }).ToListAsync();
        }
    }
}
