using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using RestaurantSystem.Data.Context;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Enums;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public readonly IUnitOfWork _uow;
        public readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(OrderItemRegisterDTO item)
        {
            if (!ModelState.IsValid)
                return BadRequest(Json(new { Errors = ModelState.Values.SelectMany(v => v.Errors) }));

            var food = await _uow.FoodRepo.GetByIdAsync(item.FoodID);
            var ingredients = new List<IngredientDTO>();
            decimal sumIngredientsPrices = 0;

            if (item.SelectedOptionalIngredients is not null)
            {
                ingredients = await _uow.IngredientRepo.GetAllByListId(item.SelectedOptionalIngredients)
                    .Select(i => new IngredientDTO { Id = i.Id, Name = i.Name, Price = i.Price })
                    .ToListAsync();
                sumIngredientsPrices = ingredients.Sum(i => i.Price);
            }

            if (food is null)
                return BadRequest(Json(new { Errors = new List<string>() { "Comida não encontrada!" } }));

            var orderItemSessionDTO = new OrderItemSessionDTO()
            {
                Food = food,
                Quantity = item.Quantity,
                OptionalIngredientsSelected = ingredients,
                TotalPrice = (food.BasePrice + sumIngredientsPrices) * item.Quantity
            };

            var order = new OrderSessionDTO()
            {
                OrderItems = new Collection<OrderItemSessionDTO>()
            };

            string? orderSession = HttpContext.Session.GetString("Order");

            if (orderSession is not null && orderSession != "null")
            {
                order = JsonConvert.DeserializeObject<OrderSessionDTO>(orderSession);
            }

            order.OrderItems.Add(orderItemSessionDTO);
            order.TotalPrice = order.OrderItems.Sum(i => i.TotalPrice);

            HttpContext.Session.SetString("Order", JsonConvert.SerializeObject(order, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

            return Ok();
        }

        public IActionResult GetOrder()
        {
            string? orderSession = HttpContext.Session.GetString("Order");
            if (orderSession is null)
                return Json(new { });

            var order = JsonConvert.DeserializeObject<OrderSessionDTO>(orderSession);

            return Json(order);
        }

        public async Task<IActionResult> Buy()
        {
            string? orderSession = HttpContext.Session.GetString("Order");
            if (orderSession is null)
                return BadRequest();

            var order = JsonConvert.DeserializeObject<OrderSessionDTO>(orderSession);
            if (order is null)
                return BadRequest();

            var orderEntity = new Order()
            {
                OrderedItems = new Collection<OrderItem>()
            };

            foreach (var item in order.OrderItems)
            {
                var food = await _uow.FoodRepo.GetByIdAsync(item.Food.Id);
                decimal sumIngredientsPrices = 0;

                List<Ingredient>? ingredients = null;
                if (item.OptionalIngredientsSelected is not null)
                {
                    IEnumerable<long> ingredientIds = item.OptionalIngredientsSelected?.Select(i => (long)i.Id) ?? [];
                    ingredients = _uow.IngredientRepo.GetAllByListId(ingredientIds).ToList();

                    sumIngredientsPrices += ingredients.Sum(i => i.Price);

                }


                var orderItemEntity = new OrderItem()
                {
                    Food = food,
                    OptionalIngredientsSelected = ingredients,
                    Quantity = item.Quantity,
                    TotalPrice = (food.BasePrice + sumIngredientsPrices) * item.Quantity
                };

                orderEntity.OrderedItems.Add(orderItemEntity);
            }

            orderEntity.TotalPrice = orderEntity.OrderedItems.Sum(i => i.TotalPrice);
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            orderEntity.User = user;
            orderEntity.OrderDate = DateTime.UtcNow;
            orderEntity.Status = Status.Pending;

            await _uow.OrderRepo.AddAsync(orderEntity);
            await _uow.CommitAsync();

            HttpContext.Session.SetString("Order", "null");

            return Ok();
        }
    }
}
