using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.ShoppingCartDtos;

namespace FastFoodShop.Web.Services.IServices;

public interface IOrderService
{
    Task<ResponseDto?> CreateOrder(CartDto cartDto);
    Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);
    Task<ResponseDto?> ValidateStripeSession(int orderHeaderId);
}