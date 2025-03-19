using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using RestaurantSystem.Data;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    public class OrderController : Controller
    {
        public readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(OrderItemRegisterDTO item)
        {
            if (!ModelState.IsValid)
                return BadRequest(Json(new { Errors = ModelState.Values.SelectMany(v => v.Errors) }));

            var food = await _context.Foods.SingleOrDefaultAsync(f => f.Id == item.FoodID);
            var ingredients = new List<IngredientDTO>();
            decimal sumIngredientsPrices = 0;

            if (item.SelectedOptionalIngredients is not null)
            {
                ingredients = await _context.Ingredients
                    .Where(i => item.SelectedOptionalIngredients.Contains(i.Id))
                    .Select(i => new IngredientDTO { Id = i.Id, Name = i.Name, Price = i.Price})
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
                return Json(new {});

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

            var orderEntity = new Order ()
            {
                OrderedItems = new Collection<OrderItem>()
            };

            foreach (var item in order.OrderItems)
            {
                var food = await _context.Foods.SingleAsync(f => f.Id == item.Food.Id);
                decimal sumIngredientsPrices = 0;

                List<Ingredient>? ingredients = null;
                if (item.OptionalIngredientsSelected is not null)
                {
                    ingredients = _context.Ingredients
                        .Where(i => 
                            item.OptionalIngredientsSelected
                                .Select(i => i.Id)
                                .Contains(i.Id))
                        .ToList();

                    sumIngredientsPrices += ingredients.Sum(i => i.Price);

                }


                var orderItemEntity = new OrderItem()
                {
                    Food = food,
                    OptionalIngredientsSelected = ingredients,
                    Quantity = item.Quantity,
                    TotalPrice = ( food.BasePrice + sumIngredientsPrices) * item.Quantity,
                };

                orderEntity.OrderedItems.Add(orderItemEntity);
            }
            
            orderEntity.TotalPrice = orderEntity.OrderedItems.Sum(i => i.TotalPrice);

            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("Order", "null");


            return Ok();
        }
    }
}
