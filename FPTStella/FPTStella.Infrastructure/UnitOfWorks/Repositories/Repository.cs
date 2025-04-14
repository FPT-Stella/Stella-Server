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
        private readonly IMongoCollection<T> _collection;

        public Repository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);

            if (typeof(T) == typeof(FPTStella.Domain.Entities.User))
            {
                var indexKeys = Builders<T>.IndexKeys.Ascending("username");
                var indexOptions = new CreateIndexOptions { Unique = true };
                _collection.Indexes.CreateOne(new CreateIndexModel<T>(indexKeys, indexOptions));
            }
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return null;
            }
            var filter = Builders<T>.Filter.Eq("Id", guidId) & Builders<T>.Filter.Eq("del_flg", false);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> filter)
        {
            var activeFilter = Builders<T>.Filter.Eq("del_flg", false);
            var combinedFilter = Builders<T>.Filter.And(Builders<T>.Filter.Where(filter), activeFilter);
            return await _collection.Find(combinedFilter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> filter)
        {            
            var activeFilter = Builders<T>.Filter.Eq("del_flg", false);
            var combinedFilter = Builders<T>.Filter.And(Builders<T>.Filter.Where(filter), activeFilter);
            return await _collection.Find(combinedFilter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var filter = Builders<T>.Filter.Eq("del_flg", false);
            return await _collection.Find(filter).ToListAsync();
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
            {
                throw new ArgumentException("Invalid GUID format.", nameof(id));
            }
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.UpdDate = DateTime.UtcNow;
            }
            var filter = Builders<T>.Filter.Eq("Id", guidId) & Builders<T>.Filter.Eq("del_flg", false);
            var result = await _collection.ReplaceOneAsync(filter, entity);
            if (result.MatchedCount == 0)
            {
                throw new Exception("Entity not found or already deleted.");
            }
        }

        public async Task DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                throw new ArgumentException("Invalid GUID format.", nameof(id));
            }
            var filter = Builders<T>.Filter.Eq("Id", guidId) & Builders<T>.Filter.Eq("del_flg", false);
            var update = Builders<T>.Update
                .Set("del_flg", true)
                .Set("upd_date", DateTime.UtcNow);
            var result = await _collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 0)
            {
                throw new Exception("Entity not found or already deleted.");
            }
        }
    }
}