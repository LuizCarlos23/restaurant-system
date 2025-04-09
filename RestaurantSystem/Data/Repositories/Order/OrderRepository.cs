
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;

namespace RestaurantSystem.Data.Repositories.Order
{
    public class OrderRepository : Repository<Models.Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Models.Order> GetAllWithItems()
        {
            return _dbSet.Include(o => o.OrderedItems);
        }
    }
}
