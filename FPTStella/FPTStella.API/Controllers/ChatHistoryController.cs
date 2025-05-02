using FPTStella.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FPTStella.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatHistoryController : ControllerBase
    {
        private readonly IChatHistoryService _chatHistoryService;

        public ChatHistoryController(IChatHistoryService chatHistoryService)
        {
            _chatHistoryService = chatHistoryService;
        }

        /// <summary>
        /// Get chat history by session ID
        /// </summary>
        /// <param name="sessionId">The session ID</param>
        /// <returns>Chat history</returns>
        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetChatHistory(string sessionId)
        {
            var chatHistory = await _chatHistoryService.GetChatHistoryBySessionIdAsync(sessionId);
            if (chatHistory == null)
                return NotFound("Chat history not found.");

            return Ok(chatHistory);
        }
    }
}