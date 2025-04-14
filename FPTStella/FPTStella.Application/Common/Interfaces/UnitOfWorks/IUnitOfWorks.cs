using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Lấy repository generic cho một entity.
        /// </summary>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        /// <summary>
        /// Lưu các thay đổi (commit transaction nếu có).
        /// </summary>
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
