using RestaurantSystem.Data.Repositories.Food;
using RestaurantSystem.Data.Repositories.Ingredient;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IIngredientRepository IngredientRepo { get; }
        IFoodRepository FoodRepo { get; }

        Task CommitAsync();
    }
}
