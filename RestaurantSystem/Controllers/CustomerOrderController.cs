using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data.Context;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class CustomerOrderController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;

        public CustomerOrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = _context.Orders
                .Include(o => o.OrderedItems)
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
