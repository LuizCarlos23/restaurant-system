using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Ingredient> IngredientRepo { get; }

        Task CommitAsync();
    }
}
