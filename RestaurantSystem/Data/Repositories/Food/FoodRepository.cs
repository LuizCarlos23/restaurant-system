
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;

namespace RestaurantSystem.Data.Repositories.Food
{
    public class FoodRepository : Repository<Models.Food>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<Models.Food?> GetByIdWithIngredientsAsync(long id)
        {
            return _context.Foods.Include(f => f.OptionalIngredients).SingleOrDefaultAsync(f => f.Id == id);
        }
    }
}
