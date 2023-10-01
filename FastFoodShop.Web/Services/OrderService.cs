using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Models.ShoppingCartDtos;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;

namespace FastFoodShop.Web.Services;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;
    private const string OrderUrl = "/api/order/";
    private const string PaymentUrl = "/api/payment/";
    private const string Separator = "/";
    
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
            Url = SD.OrderAPIBase + OrderUrl
        });
    }
    
    public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = stripeRequestDto,
            Url = SD.OrderAPIBase + PaymentUrl
        });
    }
    
    public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.OrderAPIBase + PaymentUrl + orderHeaderId
        });
    }

    public async Task<ResponseDto?> GetAllOrder(string? userId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.OrderAPIBase + OrderUrl + $"?userId={userId}"
        });
    }

    public async Task<ResponseDto?> GetOrder(int orderId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.OrderAPIBase + OrderUrl + orderId
        });
    }

    public async Task<ResponseDto?> UpdateOrderStatus(int orderId, int newStatus)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.PUT,
            Url = SD.OrderAPIBase + OrderUrl + orderId + Separator + newStatus
        });
    }
}