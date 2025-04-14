using System.Linq;
using System.Linq.Expressions;

namespace FPTStella.Application.Common.Interfaces.UnitOfWorks
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T entity);
        Task InsertManyAsync(IEnumerable<T> entities);
        Task ReplaceAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
