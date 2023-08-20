using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;

namespace FastFoodShop.Web.Services;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;
    private const string Url = "/api/shopping-cart/";

    public CartService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = SD.ShoppingCartAPIBase + Url + "apply-coupon"
        });
    }

    public async Task<ResponseDto?> EmailCart(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = SD.ShoppingCartAPIBase + Url + "email-cart-request"
        });
    }

    public async Task<ResponseDto?> GetCartByUserIdAsnyc(string userId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.ShoppingCartAPIBase + Url + userId
        });
    }


    public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.DELETE,
            Url = SD.ShoppingCartAPIBase + Url + cartDetailsId
        });
    }


    public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = SD.ShoppingCartAPIBase + Url
        });
    }
}