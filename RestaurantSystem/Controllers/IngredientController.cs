using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        public readonly IUnitOfWork _uow;

        public IngredientController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var ingredients = await _uow.IngredientRepo.GetAllAsync();

            return View(ingredients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IngredientDTO ingredient)
        {
            if (!ModelState.IsValid)
                View(ingredient);

            await _uow.IngredientRepo.AddAsync(new Ingredient { Name = ingredient.Name, Price = ingredient.Price });
            await _uow.CommitAsync();

            TempData["SuccessMessage"] = "Ingrediente cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(long id)
        {
            var ingredient = await _uow.IngredientRepo.GetByIdAsync(id);
            if (ingredient is null)
                return NotFound();

            var dto = new IngredientDTO()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Price = ingredient.Price
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, IngredientDTO ingredient)
        {
            if (ingredient.Id != id)
                return NotFound();
            else if (!ModelState.IsValid)
                return View(ingredient);


            _uow.IngredientRepo.Update(new Ingredient()
            {
                Id = (long)ingredient.Id,
                Name = ingredient.Name,
                Price = ingredient.Price
            });

            await _uow.CommitAsync();

            TempData["SuccessMessage"] = "Ingrediente alterado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var ingredient = await _uow.IngredientRepo.GetByIdAsync(id);
            if (ingredient is null)
                return BadRequest();

            _uow.IngredientRepo.Delete(ingredient);
            await _uow.CommitAsync();

            return Ok();
        }
    }
}
