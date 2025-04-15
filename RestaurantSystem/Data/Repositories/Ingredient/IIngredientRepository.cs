using RestaurantSystem.DTOs;

namespace RestaurantSystem.Data.Repositories.Ingredient
{
    public interface IIngredientRepository : IRepository<Models.Ingredient>
    {
        IQueryable<Models.Ingredient> GetAllByListId(IEnumerable<long> ids);
        Task<IEnumerable<IngredientSelectApresentationDTO>> GetAllForSelectAsync();        
        IQueryable<Models.Ingredient> GetOptionalForFoodId(long foodId);
        IQueryable<Models.Ingredient> GetExclusiveForFoodId(long foodId);
    }
}
