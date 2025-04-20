using FPTStella.Application.Common.Interfaces.Persistences;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private readonly ConcurrentDictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _repositories = new ConcurrentDictionary<Type, object>();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return (IRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity), _ =>
                new Repository<TEntity>(_database, typeof(TEntity).Name));
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            // MongoDB không cần commit như SQL, nhưng có thể thêm logic transaction nếu cần.
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _repositories.Clear();
                _disposed = true;
            }
        }
    }
}
