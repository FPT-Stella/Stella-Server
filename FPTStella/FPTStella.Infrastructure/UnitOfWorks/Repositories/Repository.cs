using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.UnitOfWorks.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        #region Private Helpers

        private static FilterDefinition<T> GetIdFilter(Guid id)
        {
            return Builders<T>.Filter.Eq("Id", id);
        }

        private static FilterDefinition<T> GetActiveFilter()
        {
            return Builders<T>.Filter.Eq("del_flg", false);
        }

        private static FilterDefinition<T> CombineWithActiveFilter(Expression<Func<T, bool>> predicate)
        {
            var expressionFilter = Builders<T>.Filter.Where(predicate);
            return Builders<T>.Filter.And(expressionFilter, GetActiveFilter());
        }

        #endregion

        public async Task<T?> GetByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId)) return null;

            var filter = Builders<T>.Filter.And(GetIdFilter(guidId), GetActiveFilter());
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate)
        {
            var filter = CombineWithActiveFilter(predicate);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> predicate)
        {
            var filter = CombineWithActiveFilter(predicate);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(GetActiveFilter()).ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.Id = baseEntity.Id == Guid.Empty ? Guid.NewGuid() : baseEntity.Id;
                baseEntity.InsDate = DateTime.UtcNow;
                baseEntity.UpdDate = DateTime.UtcNow;
                baseEntity.DelFlg = false;
            }

            await _collection.InsertOneAsync(entity);
        }

        public async Task InsertManyAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.Id = baseEntity.Id == Guid.Empty ? Guid.NewGuid() : baseEntity.Id;
                    baseEntity.InsDate = DateTime.UtcNow;
                    baseEntity.UpdDate = DateTime.UtcNow;
                    baseEntity.DelFlg = false;
                }
            }

            await _collection.InsertManyAsync(entities);
        }

        public async Task ReplaceAsync(string id, T entity)
        {
            if (!Guid.TryParse(id, out var guidId))
                throw new ArgumentException("Invalid GUID format.", nameof(id));

            if (entity is BaseEntity baseEntity)
                baseEntity.UpdDate = DateTime.UtcNow;

            var filter = Builders<T>.Filter.And(GetIdFilter(guidId), GetActiveFilter());
            var result = await _collection.ReplaceOneAsync(filter, entity);

            if (result.MatchedCount == 0)
                throw new Exception("Entity not found or already deleted.");
        }

        public async Task DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
                throw new ArgumentException("Invalid GUID format.", nameof(id));

            var filter = Builders<T>.Filter.And(GetIdFilter(guidId), GetActiveFilter());
            var update = Builders<T>.Update
                .Set("del_flg", true)
                .Set("upd_date", DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
                throw new Exception("Entity not found or already deleted.");
        }
        public async Task<PagedResult<T>> SearchAsync(
            string searchTerm,
            PaginationParams paginationParams,
            string[]? searchableFields = null,
            bool exactMatch = false)
        {
            var result = new PagedResult<T>
            {
                CurrentPage = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If no search term is provided, just return a paginated list
                result.TotalCount = (int)await _collection.CountDocumentsAsync(GetActiveFilter());
                result.Items = await _collection.Find(GetActiveFilter())
                    .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                    .Limit(paginationParams.PageSize)
                    .ToListAsync();
            }
            else
            {
                // Create a search filter based on the searchable fields
                FilterDefinition<T> searchFilter;

                if (searchableFields == null || searchableFields.Length == 0)
                {
                    // Automatically determine which fields to search on (string fields)
                    var stringProperties = typeof(T).GetProperties()
                        .Where(p => p.PropertyType == typeof(string))
                        .Select(p => GetBsonElementName(p))
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToArray();

                    searchFilter = BuildSearchFilter(stringProperties, searchTerm, exactMatch);
                }
                else
                {
                    // Use the provided searchable fields
                    searchFilter = BuildSearchFilter(searchableFields, searchTerm, exactMatch);
                }

                // Combine with active filter
                var filter = Builders<T>.Filter.And(searchFilter, GetActiveFilter());

                // Execute the query with pagination
                result.TotalCount = (int)await _collection.CountDocumentsAsync(filter);
                result.Items = await _collection.Find(filter)
                    .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                    .Limit(paginationParams.PageSize)
                    .ToListAsync();
            }

            result.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)paginationParams.PageSize);
            return result;
        }

        private string GetBsonElementName(PropertyInfo property)
        {
            var bsonElementAttr = property.GetCustomAttribute<MongoDB.Bson.Serialization.Attributes.BsonElementAttribute>();
            return bsonElementAttr?.ElementName ?? property.Name;
        }

        private FilterDefinition<T> BuildSearchFilter(string[] fieldNames, string searchTerm, bool exactMatch)
        {
            var filters = new List<FilterDefinition<T>>();

            foreach (var field in fieldNames)
            {
                if (exactMatch)
                {
                    filters.Add(Builders<T>.Filter.Eq(field, searchTerm));
                }
                else
                {
                    var regexPattern = new BsonRegularExpression(
                        new Regex(Regex.Escape(searchTerm), RegexOptions.IgnoreCase));
                    filters.Add(Builders<T>.Filter.Regex(field, regexPattern));
                }
            }

            if (filters.Count == 0)
            {
                return Builders<T>.Filter.Where(_ => false);
            }
            return Builders<T>.Filter.Or(filters);
        }
    }
}