using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;
using FastFoodShop.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace FastFoodShop.Services.ShoppingCartAPI.Service;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CouponService(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }

    public async Task<CouponDto> GetCouponAsync(string couponCode)
    {
        HttpClient client = _httpClientFactory.CreateClient("Coupon");
        HttpResponseMessage response = await client.GetAsync($"/api/coupon/{couponCode}");
        string apiContet = await response.Content.ReadAsStringAsync();
        ResponseDto resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
        if (resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
        }
        return new CouponDto();
    }
}