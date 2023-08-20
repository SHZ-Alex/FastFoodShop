using FastFoodShop.Web.Models;

namespace FastFoodShop.Web.Services.IServices;

public interface ICartService
{
    Task<ResponseDto?> GetCartByUserIdAsnyc(string userId);
    Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
    Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
    Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
    Task<ResponseDto?> EmailCart(CartDto cartDto);
}