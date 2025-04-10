using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestaurantSystem.Data.Repositories;
using RestaurantSystem.DTOs;
using RestaurantSystem.Enums;
using RestaurantSystem.Models;
using RestaurantSystem.Services.Interfaces;

namespace RestaurantSystem.Services
{
    public class OrderService : IOrderService
    {
        public readonly IUnitOfWork _uow;
        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OrderItemDTO> CreateOrderItemDto(OrderItemSessionDTO item, Food food)
        {
            var ingredients = new List<Ingredient>();
            decimal sumIngredientsPrices = 0;

            if (item.OptionalIngredientsSelectedIds is not null)
            {
                ingredients = await _uow.IngredientRepo.GetAllByListId(item.OptionalIngredientsSelectedIds).ToListAsync();
                sumIngredientsPrices = ingredients.Sum(i => i.Price);
            }

            return new OrderItemDTO()
            {
                Food = food,
                Quantity = item.Quantity,
                OptionalIngredientsSelected = ingredients,
                TotalPrice = (food.BasePrice + sumIngredientsPrices) * item.Quantity
            };
        }

        public OrderSessionDTO AddOrderItemToOrderSessionDto(OrderItemSessionDTO orderItemSessionDto, OrderSessionDTO? orderSessionDto = null)
        {
            OrderSessionDTO order;
            if (orderSessionDto is null)
            {
                order = new OrderSessionDTO()
                {
                    OrderItems = new Collection<OrderItemSessionDTO>() { orderItemSessionDto }
                };
            } else
            {
                order = orderSessionDto;
                order.OrderItems.Add(orderItemSessionDto);
            }

            return order;
        }

        public async Task<OrderDto> CreateOrderDtoFromOrderSessionDto(OrderSessionDTO orderSessionDto)
        {
            var order = new OrderDto()
            {
                OrderedItems = new Collection<OrderItemDTO>()
            };

            foreach (var item in orderSessionDto.OrderItems)
            {
                var food = await _uow.FoodRepo.GetByIdAsync(item.FoodId);
                if (food is null)
                    throw new Exception("Comida não encontrada");

                var orderItemDto = await CreateOrderItemDto(item, food);

                var orderItemEntity = new OrderItemDTO()
                {
                    Food = food,
                    OptionalIngredientsSelected = orderItemDto.OptionalIngredientsSelected,
                    Quantity = item.Quantity,
                    TotalPrice = orderItemDto.TotalPrice
                };

                order.OrderedItems.Add(orderItemEntity);
            }

            order.TotalPrice = order.OrderedItems.Sum(i => i.TotalPrice);
            return order;
        }

        public async Task<Order> CreateOrderFromOrderSessionDto(OrderSessionDTO orderSessionDto, ApplicationUser user)
        {
            var orderDto = await CreateOrderDtoFromOrderSessionDto(orderSessionDto);

            var order = new Order() { OrderedItems = new Collection<OrderItem>() };

            order.OrderedItems = orderDto.OrderedItems.Select(dto => new OrderItem()
            {
                Food = dto.Food,
                OptionalIngredientsSelected = dto.OptionalIngredientsSelected,
                Quantity = dto.Quantity,
                TotalPrice = dto.TotalPrice
                
            }).ToList();

            order.User = user;
            order.OrderDate = DateTime.UtcNow;
            order.Status = Status.Pending;
            order.TotalPrice = orderDto.TotalPrice;

            return order;
        }
    }
}
