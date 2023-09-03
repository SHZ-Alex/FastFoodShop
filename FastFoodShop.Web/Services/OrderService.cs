using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
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

}