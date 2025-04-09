using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using RestaurantSystem.Data.Context;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class FoodController : Controller
    {
        public readonly IUnitOfWork _uow;

        public FoodController(IUnitOfWork uow, ApplicationDbContext context)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _uow.FoodRepo.GetAllAsync();
            return View(foods);
        }

        public async Task<IActionResult> Create()
        {
            var ingredients = await _uow.IngredientRepo.GetAllForSelectAsync();

            ViewBag.OptionalIngredients = new MultiSelectList(ingredients, "Id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodDTO food)
        {
            if (!ModelState.IsValid)
                return View(food);

            var entity = new Food()
            {
                Name = food.Name,
                Description = food.Description,
                BasePrice = food.BasePrice
            };

            if (food.Image is not null)
            {
                using (var stream = new MemoryStream())
                {
                    await food.Image.CopyToAsync(stream);
                    entity.Image = stream.ToArray();
                    entity.ImageMimeType = food.Image.ContentType;
                    entity.ImageFileName = food.Image.FileName;
                }
            }


            if (food.OptionalIngredients is not null)
            {
                var optionalIngredients = await _uow.IngredientRepo.GetAllByListId(food.OptionalIngredients).ToListAsync();
                entity.OptionalIngredients = optionalIngredients;
            }


            await _uow.FoodRepo.AddAsync(entity);
            await _uow.CommitAsync();

            ViewData["SuccessMessage"] = "Comida cadastrada com sucesso!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(long id)
        {
            var food = await _uow.FoodRepo.GetByIdWithIngredientsAsync(id);

            if (food is null)
                return NotFound();

            return View(food);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var food = await _uow.FoodRepo.GetByIdWithIngredientsAsync(id);

            if (food is null)
                return NotFound();

            List<long> selectedIngredients = [];
            if (food.OptionalIngredients is not null)
            {
                selectedIngredients = food.OptionalIngredients.Select(i => i.Id).ToList();
            }

            var ingredients = await _uow.IngredientRepo.GetAllForSelectAsync();

            ViewBag.OptionalIngredients = new MultiSelectList(ingredients, "Id", "Description", selectedIngredients);

            var dto = new FoodDTO()
            {
                Id = food.Id,
                Name = food.Name,
                Description = food.Description,
                BasePrice = food.BasePrice
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, FoodDTO food)
        {
            if (food.Id != id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(food);


            var entity = await _uow.FoodRepo.GetByIdWithIngredientsAsync(id);
            if (entity is null)
                return NotFound();

            entity.Name = food.Name;
            entity.Description = food.Description;
            entity.BasePrice = food.BasePrice;

            if (food.Image is not null)
            {
                using (var stream = new MemoryStream())
                {
                    await food.Image.CopyToAsync(stream);
                    entity.Image = stream.ToArray();
                    entity.ImageMimeType = food.Image.ContentType;
                    entity.ImageFileName = food.Image.FileName;
                }
            }


            if (food.OptionalIngredients is not null)
            {
                var ingredients = await _uow.IngredientRepo.GetAllByListId(food.OptionalIngredients).ToListAsync();
                entity.OptionalIngredients?.Clear();
                entity.OptionalIngredients.AddRange(ingredients);
            }

            _uow.FoodRepo.Update(entity);
            await _uow.CommitAsync();

            ViewData["SuccessMessage"] = "Comida atualizada com sucesso!";

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveImageFood(long foodId)
        {
            var food = await _uow.FoodRepo.GetByIdAsync(foodId);
            if (food is null)
                return NotFound();

            food.Image = null;
            food.ImageFileName = null;
            food.ImageMimeType = null;

            await _uow.CommitAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var food = await _uow.FoodRepo.GetByIdWithIngredientsAsync(id);

            if (food is null)
                return NotFound();

            _uow.FoodRepo.Delete(food);
            await _uow.CommitAsync();

            return Ok();
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetFood(long id)
        {
            var food = await _uow.FoodRepo.GetByIdWithIngredientsAsync(id);

            return Json(food, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve });
        }

        [AllowAnonymous]
        public async Task<IActionResult?> GetImage(long id)
        {
            var food = await _uow.FoodRepo.GetByIdAsync(id);
            if (food is null || food?.ImageMimeType is null)
                return NotFound();

            return File(food.Image, food.ImageMimeType);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveIngredient(long foodId, long ingredientId)
        {
            var food = await _uow.FoodRepo.GetByIdWithIngredientsAsync(foodId);
            if (food is null || food.OptionalIngredients is null)
                return NotFound();

            var ingredient = food.OptionalIngredients.SingleOrDefault(i => i.Id == ingredientId);

            if (ingredient is null)
                return NotFound();

            food.OptionalIngredients.Remove(ingredient);
            await _uow.CommitAsync();

            return NoContent();
        }
    }
}
