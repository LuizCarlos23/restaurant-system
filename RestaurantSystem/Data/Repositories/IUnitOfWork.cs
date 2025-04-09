using RestaurantSystem.Data.Repositories.Food;
using RestaurantSystem.Data.Repositories.Ingredient;
using RestaurantSystem.Data.Repositories.Order;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IIngredientRepository IngredientRepo { get; }
        IFoodRepository FoodRepo { get; }

        IOrderRepository OrderRepo { get; }

        Task CommitAsync();
    }
}
