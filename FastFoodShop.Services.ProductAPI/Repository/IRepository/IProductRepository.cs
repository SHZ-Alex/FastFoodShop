using FastFood.Services.ProductAPI.Models;
using FastFoodShop.Services.CouponAPI.Repository.IRepository;

namespace FastFood.Services.ProductAPI.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product entity);
}