using FPTStella.Domain.Common;
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
        /// <summary>
        /// Searches for entities based on search criteria with pagination support.
        /// </summary>
        /// <param name="searchTerm">The text to search for across searchable fields</param>
        /// <param name="paginationParams">Pagination parameters (page number and page size)</param>
        /// <param name="searchableFields">Optional array of field names to search in. If null, searches in all text fields.</param>
        /// <param name="exactMatch">If true, searches for exact matches. If false, uses contains/partial matching.</param>
        /// <returns>A paged result containing the matching entities</returns>
        Task<PagedResult<T>> SearchAsync(
            string searchTerm,
            PaginationParams paginationParams,
            string[]? searchableFields = null,
            bool exactMatch = false);
    }
}
