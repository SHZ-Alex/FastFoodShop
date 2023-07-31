using System.Linq.Expressions;

namespace FastFoodShop.Services.CouponAPI.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(Expression<Func<T, bool>> filter = null);
    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAsync();
}