using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
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

        public OrderItemDTO CreateOrderItemDto(OrderItemSessionDTO item, Food food)
        {
            var optionalIngredient = new List<Ingredient>();
            Ingredient? exclusiveIngredient = null;
            decimal sumIngredientsPrices = 0;


            if (item.OptionalIngredientsSelectedIds is not null)
            {
                var foodIngredients = _uow.IngredientRepo.GetOptionalForFoodId(food.Id);

                if (foodIngredients is null)
                    throw new InvalidOperationException("Ingredientes inválidos foram selecionados para essa comida.");

                optionalIngredient = foodIngredients
                    .Where(i => item.OptionalIngredientsSelectedIds.Contains(i.Id))
                    .ToList();


                sumIngredientsPrices = optionalIngredient.Sum(i => i.Price);
            }

            if (item.ExclusiveIngredientSelectedId is not null)
            {
                var foodIngredients = _uow.IngredientRepo.GetExclusiveForFoodId(food.Id);
                if (foodIngredients is null)
                    throw new InvalidOperationException("Ingredientes inválidos foram selecionados para essa comida.");

                exclusiveIngredient = foodIngredients
                    .Where(i => i.Id == item.ExclusiveIngredientSelectedId)
                    .SingleOrDefault();

                sumIngredientsPrices += exclusiveIngredient?.Price ?? 0;
            }

            return new OrderItemDTO()
            {
                Food = food,
                Quantity = item.Quantity,
                OptionalIngredientsSelected = optionalIngredient,
                ExclusiveIngredientSelected = exclusiveIngredient,
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

                var orderItemDto = CreateOrderItemDto(item, food);

                order.OrderedItems.Add(orderItemDto);
            }

            order.TotalPrice = order.OrderedItems.Sum(i => i.TotalPrice);
            return order;
        }

        public async Task<Order> CreateOrderFromOrderSessionDto(OrderSessionDTO orderSessionDto, ApplicationUser user)
        {
            var orderDto = await CreateOrderDtoFromOrderSessionDto(orderSessionDto);

            var order = new Order
            {
                OrderedItems = orderDto.OrderedItems.Select(dto => new OrderItem()
                {
                    Food = dto.Food,
                    OptionalIngredientsSelected = dto.OptionalIngredientsSelected,
                    ExclusiveIngredientSelected = dto.ExclusiveIngredientSelected,
                    Quantity = dto.Quantity,
                    TotalPrice = dto.TotalPrice

                }).ToList(),

                User = user,
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                TotalPrice = orderDto.TotalPrice
            };

            return order;
        }
    }
}
