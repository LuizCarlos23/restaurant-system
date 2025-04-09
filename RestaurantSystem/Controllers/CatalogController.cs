using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    public class CatalogController : Controller
    {
        public IUnitOfWork _uow;

        public CatalogController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var catalog = await _uow.FoodRepo.GetCatalogAsync();
            return View(catalog);
        }
    }
}
