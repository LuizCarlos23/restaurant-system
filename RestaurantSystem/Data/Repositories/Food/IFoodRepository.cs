using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories.Food
{
    public interface IFoodRepository : IRepository<Models.Food>
    {
        Task<Models.Food?> GetByIdWithIngredientsAsync(long id);
        Task<IEnumerable<CatalogViewModel>> GetCatalogAsync();
    }
}
