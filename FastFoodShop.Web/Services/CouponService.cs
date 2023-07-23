using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;

namespace FastFoodShop.Web.Services;

public class CouponService : ICouponService
{
    private const string Url = "/api/coupon/";
    private readonly IBaseService _baseService;

    public CouponService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto> GetCouponAsync(string couponCode)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = SD.CouponAPIBase + Url + couponCode
        });
    }

    public async Task<ResponseDto> GetAllCouponAsync()
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = SD.CouponAPIBase + Url
        });
    }

    public async Task<ResponseDto> GetCouponByIdAsync(int couponId)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = SD.CouponAPIBase + Url + couponId,
        });
    }

    public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = couponDto,
            Url = SD.CouponAPIBase + Url,
        });
    }

    public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.PUT,
            Data = couponDto,
            Url = SD.CouponAPIBase + Url,
        });
    }

    public async Task<ResponseDto> DeleteCouponAsync(int couponId)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.DELETE,
            Url = SD.CouponAPIBase + Url + couponId,
        });
    }
}