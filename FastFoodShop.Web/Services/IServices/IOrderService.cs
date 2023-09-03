using FastFoodShop.Web.Models;

namespace FastFoodShop.Web.Services.IServices;

public interface IOrderService
{
    Task<ResponseDto?> CreateOrder(CartDto cartDto);
}