
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;

namespace RestaurantSystem.Data.Repositories.Order
{
    public class OrderRepository : Repository<Models.Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Models.Order> GetAllByUserId(string userId)
        {
            return _dbSet.Where(o => o.User.Id == userId);
        }

        public IQueryable<Models.Order> GetByIdWithItems(long id, string? userId)
        {
            var query = (userId is not null) ? GetAllByUserId(userId) : _dbSet;

            return query.Where(o => o.Id == id)
                .Include(o => o.OrderedItems)
                    .ThenInclude(oi => oi.Food)
                .Include(o => o.OrderedItems)
                    .ThenInclude(oi => oi.OptionalIngredientsSelected)
                .Include(o => o.OrderedItems)
                    .ThenInclude(oi => oi.ExclusiveIngredientSelected);
        }

    }
}
