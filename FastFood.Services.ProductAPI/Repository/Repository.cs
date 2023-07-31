using System.Linq.Expressions;
using FastFood.Services.ProductAPI.Data;
using FastFoodShop.Services.CouponAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.ProductAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _db;
    private DbSet<T> dbSet;

    public Repository(AppDbContext db)
    {
        _db = db;
        dbSet = db.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
        dbSet.Add(entity);
        await SaveAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
    {
        IQueryable<T> query = dbSet;

        return await query
            .SingleOrDefaultAsync(filter);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        IQueryable<T> query = dbSet;
        return await query.ToArrayAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}