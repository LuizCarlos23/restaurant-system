using RestaurantSystem.DTOs;

namespace RestaurantSystem.Data.Repositories.Ingredient
{
    public interface IIngredientRepository : IRepository<Models.Ingredient>
    {
        Task<ICollection<Models.Ingredient>> GetIngredientsByListIdAsync(IEnumerable<long> ids);
        Task<IEnumerable<IngredientSelectApresentationDTO>> GetAllForSelectAsync();
    }
}
