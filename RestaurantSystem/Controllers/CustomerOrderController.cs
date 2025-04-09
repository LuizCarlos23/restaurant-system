using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class CustomerOrderController : Controller
    {
        public readonly IUnitOfWork _uow;
        public readonly UserManager<ApplicationUser> _userManager;

        public CustomerOrderController(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = _uow.OrderRepo.GetAllWithItems()
                .Where(o => o.User.Id == user.Id)
                .Select(o => new CustomerOrderDto() { 
                        Id = o.Id, 
                        OrderDate = o.OrderDate.ToLocalTime(), 
                        Status = o.Status, 
                        TotalPrice = o.TotalPrice 
                    }
                )
                .ToList();

            return View(orders);
        }
    }
}
