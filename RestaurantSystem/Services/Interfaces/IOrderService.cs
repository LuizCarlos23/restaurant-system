using RestaurantSystem.DTOs;
using RestaurantSystem.Models;

namespace RestaurantSystem.Services.Interfaces
{
    public interface IOrderService
    {
        OrderItemDTO CreateOrderItemDto(OrderItemSessionDTO item, Models.Food food);
        OrderSessionDTO AddOrderItemToOrderSessionDto(OrderItemSessionDTO orderItemSessionDto, OrderSessionDTO? orderSessionDto = null);
        Task<Order> CreateOrderFromOrderSessionDto(OrderSessionDTO orderSessionDto, ApplicationUser user);
        Task<OrderDto> CreateOrderDtoFromOrderSessionDto(OrderSessionDTO orderSessionDto);
    }
}
