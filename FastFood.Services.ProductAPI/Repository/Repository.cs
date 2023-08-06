using System.Linq.Expressions;
using FastFood.Services.ProductAPI.Data;
using FastFoodShop.Services.CouponAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.ProductAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _db;
    private DbSet<T> _dbSet;

    public Repository(AppDbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await SaveAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
    {
        IQueryable<T> query = _dbSet;

        return await query
            .SingleOrDefaultAsync(filter);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        IQueryable<T> query = _dbSet;
        return await query.ToArrayAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}