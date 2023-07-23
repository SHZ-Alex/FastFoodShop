using FastFoodShop.Web.Models;

namespace FastFoodShop.Web.Services.IServices;

public interface ICouponService
{
    Task<ResponseDto> GetCouponAsync(string couponCode);
    Task<ResponseDto> GetAllCouponAsync();
    Task<ResponseDto> GetCouponByIdAsync(int couponId);
    Task<ResponseDto> CreateCouponAsync(CouponDto couponDto);
    Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto);
    Task<ResponseDto> DeleteCouponAsync(int couponId);
}