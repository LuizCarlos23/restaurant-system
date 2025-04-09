using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Services.Interfaces
{
    public interface IFoodService
    {
        Task<Food> ChangeImage(IFormFile image, Food entity);
        Food RemoveImage(Food entity);
        Task<Food> CreateFoodFromDtoAsync(FoodDTO dto);
        Task<Food> EditFoodFromDtoAsync(Food entity, FoodDTO dto);
    }
}
