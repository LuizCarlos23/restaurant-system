using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Models;
using RestaurantSystem.Services.Interfaces;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public readonly IUnitOfWork _uow;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IOrderService _orderService;
        private const string OrderSessionKey = "Order";

        public OrderController(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _uow = uow;
            _orderService = orderService;
        }

        private void ClearOrderSession() => HttpContext.Session.SetString(OrderSessionKey, "null");

        private void SetOrderInSession(OrderSessionDTO order)
        {
            HttpContext.Session.SetString(OrderSessionKey, 
                JsonConvert.SerializeObject(order, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        private OrderSessionDTO? GetOrderFromSession()
        {
            string? orderSession = HttpContext.Session.GetString(OrderSessionKey);

            if (orderSession is null || orderSession == "null")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<OrderSessionDTO>(orderSession);
            } catch (JsonException)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(OrderItemSessionDTO item)
        {
            if (!ModelState.IsValid)
                return BadRequest(Json(new { Errors = ModelState.Values.SelectMany(v => v.Errors) }));

            var food = await _uow.FoodRepo.GetByIdAsync(item.FoodId);

            if (food is null)
                return BadRequest(Json(new { Errors = new List<string>() { "Comida não encontrada!" } }));

            OrderSessionDTO? orderSessionConverted = GetOrderFromSession();

            var order = _orderService.AddOrderItemToOrderSessionDto(item, orderSessionConverted);

            SetOrderInSession(order);

            return Ok();
        }

        public async Task<IActionResult> GetOrder()
        {
            var orderSession = GetOrderFromSession();

            if (orderSession is null)
                return NotFound();
            try
            {
                var order = await _orderService.CreateOrderDtoFromOrderSessionDto(orderSession);
                return Json(order);
            } catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }

        public async Task<IActionResult> Buy()
        {
            var order = GetOrderFromSession();
            if (order is null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            Order orderEntity;
            try
            {
                orderEntity = await _orderService.CreateOrderFromOrderSessionDto(order, user);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }


            await _uow.OrderRepo.AddAsync(orderEntity);
            await _uow.CommitAsync();

            ClearOrderSession();

            return Ok();
        }
    }
}
