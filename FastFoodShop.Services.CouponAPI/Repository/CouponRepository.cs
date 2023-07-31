using FastFoodShop.Services.CouponAPI.Data;
using FastFoodShop.Services.CouponAPI.Models;
using FastFoodShop.Services.CouponAPI.Repository.IRepository;

namespace FastFoodShop.Services.CouponAPI.Repository;

public class CouponRepository : Repository<Coupon>, ICouponRepository
{
    private readonly AppDbContext _db;
    
    public CouponRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task UpdateAsync(Coupon entity)
    {
        _db.Coupons.Update(entity);
        await _db.SaveChangesAsync();
    }
}