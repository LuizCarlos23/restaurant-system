using RestaurantSystem.DTOs;

namespace RestaurantSystem.Data.Repositories.Order
{
    public interface IOrderRepository : IRepository<Models.Order>
    {
        IQueryable<Models.Order> GetAllWithItems();
    }
}
