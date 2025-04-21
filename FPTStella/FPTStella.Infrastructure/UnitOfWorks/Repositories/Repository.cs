using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    }
}