using FastFood.Services.ProductAPI.Data;
using FastFood.Services.ProductAPI.Models;
using FastFood.Services.ProductAPI.Repository.IRepository;

namespace FastFood.Services.ProductAPI.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _db;
    
    public ProductRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task UpdateAsync(Product entity)
    {
        _db.Products.Update(entity);
        await _db.SaveChangesAsync();
    }
}