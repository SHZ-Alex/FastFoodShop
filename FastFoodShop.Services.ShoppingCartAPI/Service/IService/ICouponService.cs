using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;

namespace FastFoodShop.Services.ShoppingCartAPI.Service.IService;

public interface ICouponService
{
    Task<CouponDto> GetCoupon(string couponCode);
}