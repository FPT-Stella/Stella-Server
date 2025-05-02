using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Domain.Entities;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class ChatHistoryService : IChatHistoryService
    {
        private readonly IChatHistoryRepository _chatHistoryRepository;

        public ChatHistoryService(IChatHistoryRepository chatHistoryRepository)
        {
            _chatHistoryRepository = chatHistoryRepository;
        }

        public async Task<ChatHistory?> GetChatHistoryBySessionIdAsync(string sessionId)
        {
            return await _chatHistoryRepository.GetBySessionIdAsync(sessionId);
        }
    }
}