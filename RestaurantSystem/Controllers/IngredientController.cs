using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    public class IngredientController : Controller
    {
        public readonly ApplicationDbContext _context;

        public IngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ingredients = _context.Ingredients.ToList();

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

            await _context.Ingredients.AddAsync(new Ingredient { Name = ingredient.Name, Price = ingredient.Price });
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ingrediente cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(long id)
        {
            var ingredient = await _context.Ingredients.SingleOrDefaultAsync(i => i.Id == id);
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
            

            _context.Ingredients.Update(new Ingredient()
            {
                Id = (long)ingredient.Id,
                Name = ingredient.Name,
                Price = ingredient.Price
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ingrediente alterado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var ingredient = await _context.Ingredients.SingleOrDefaultAsync(i => i.Id == id);
            if (ingredient is null)
                return BadRequest();

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
