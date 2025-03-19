using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    public class CatalogController : Controller
    {
        public ApplicationDbContext _context;

        public CatalogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var catalog = await _context.Foods.Select(f => 
                new CatalogViewModel() { 
                    FoodId = f.Id,
                    FoodName = f.Name, 
                    FoodDescription = f.Description, 
                    FoodPrice = f.BasePrice
                }).ToListAsync();
            
            return View(catalog);
        }
    }
}
