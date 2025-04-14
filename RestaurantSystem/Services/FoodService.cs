using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;
using RestaurantSystem.Services.Interfaces;

namespace RestaurantSystem.Services
{
    public class FoodService : IFoodService
    {
        private readonly IUnitOfWork _uow;
        public FoodService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Food> ChangeImage(IFormFile image, Food entity)
        {
            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                entity.Image = stream.ToArray();
                entity.ImageMimeType = image.ContentType;
                entity.ImageFileName = image.FileName;
            }

            return entity;
        }

        public Food RemoveImage(Food entity)
        {
            entity.Image = null;
            entity.ImageFileName = null;
            entity.ImageMimeType = null;
            return entity;
        }

        public async Task<Food> CreateFoodFromDtoAsync(FoodDTO dto)
        {
            var entity = new Food()
            {
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice
            };

            if (dto.Image is not null)
            {
                entity = await ChangeImage(dto.Image, entity);
            }


            if (dto.OptionalIngredients is not null)
            {
                var optionalIngredients = await _uow.IngredientRepo.GetAllByListId(dto.OptionalIngredients).ToListAsync();
                entity.OptionalIngredients = optionalIngredients;
            }

            if (dto.ExclusiveIngredients is not null)
            {
                var exclusiveIngredients = await _uow.IngredientRepo.GetAllByListId(dto.ExclusiveIngredients).ToListAsync();
                entity.ExclusiveIngredients = exclusiveIngredients;
            }

            return entity;
        }

        public async Task<Food> EditFoodFromDtoAsync(Food entity, FoodDTO dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.BasePrice = dto.BasePrice;

            if (dto.Image is not null)
            {
                entity = await ChangeImage(dto.Image, entity);
            }


            if (dto.OptionalIngredients is not null)
            {
                var ingredients = await _uow.IngredientRepo.GetAllByListId(dto.OptionalIngredients).ToListAsync();
                entity.OptionalIngredients?.Clear();
                entity.OptionalIngredients.AddRange(ingredients);
            }

            if (dto.ExclusiveIngredients is not null)
            {
                var ingredients = await _uow.IngredientRepo.GetAllByListId(dto.ExclusiveIngredients).ToListAsync();
                entity.ExclusiveIngredients?.Clear();
                entity.ExclusiveIngredients.AddRange(ingredients);
            }

            return entity;
        }
    }
}
