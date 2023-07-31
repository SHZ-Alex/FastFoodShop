using FastFoodShop.Services.CouponAPI.Models;

namespace FastFoodShop.Services.CouponAPI.Repository.IRepository;

public interface ICouponRepository : IRepository<Coupon>
{
    Task UpdateAsync(Coupon entity);
}