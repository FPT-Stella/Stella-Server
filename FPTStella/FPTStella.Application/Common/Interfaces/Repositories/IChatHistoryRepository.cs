using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IChatHistoryRepository : IRepository<ChatHistory>
    {
        Task<ChatHistory?> GetBySessionIdAsync(string sessionId);
    }
}
