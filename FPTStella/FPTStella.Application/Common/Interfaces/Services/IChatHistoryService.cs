using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IChatHistoryService
    {
        Task<ChatHistory?> GetChatHistoryBySessionIdAsync(string sessionId);
    }
}
