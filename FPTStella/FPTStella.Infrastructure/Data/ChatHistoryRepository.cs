using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class ChatHistoryRepository : Repository<ChatHistory>, IChatHistoryRepository
    {
        public ChatHistoryRepository(IMongoDatabase database) : base(database, "ChatHistories") { }

        public async Task<ChatHistory?> GetBySessionIdAsync(string sessionId)
        {
            var filter = Builders<ChatHistory>.Filter.Eq(ch => ch.SessionId, sessionId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}