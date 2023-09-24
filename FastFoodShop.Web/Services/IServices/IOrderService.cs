using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.ShoppingCartDtos;

namespace FastFoodShop.Web.Services.IServices;

public interface IOrderService
{
    Task<ResponseDto?> CreateOrder(CartDto cartDto);
    Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);
    Task<ResponseDto?> ValidateStripeSession(int orderHeaderId);
    Task<ResponseDto?> GetAllOrder(string? userId);
    Task<ResponseDto?> GetOrder(int orderId);
    Task<ResponseDto?> UpdateOrderStatus(int orderId, int newStatus);
}