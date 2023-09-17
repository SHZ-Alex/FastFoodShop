using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Models.ShoppingCartDtos;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;

namespace FastFoodShop.Web.Services;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;
    private const string Url = "/api/order/";
    
    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = SD.OrderAPIBase + Url
        });
    }
    
    public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = stripeRequestDto,
            Url = SD.OrderAPIBase + Url + "payment"
        });
    }
    
    public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.OrderAPIBase + Url + orderHeaderId
        });
    }

}