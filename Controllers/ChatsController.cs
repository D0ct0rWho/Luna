using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Luna.Dtos;
using Luna.Services;
using System.Security.Claims;

namespace Luna.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatsController(IChatService chatService, IMessageService messageService)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDto>>> GetChats()
        {
            var chats = await _chatService.GetUserChatsAsync(GetUserId());
            return Ok(chats);
        }

        [HttpPost]
        public async Task<ActionResult<ChatDto>> CreateChat(CreateChatRequestDto request)
        {
            var chat = await _chatService.CreateChatAsync(GetUserId(), request);
            return CreatedAtAction(nameof(GetChat), new { chatId = chat.Id }, chat);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatDto>> GetChat(Guid chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null) return NotFound();
            return Ok(chat);
        }

        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(Guid chatId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var messages = await _messageService.GetMessagesAsync(chatId, page, pageSize);
            return Ok(messages);
        }

        [HttpPost("messages")]
        public async Task<ActionResult<MessageDto>> SendMessage(SendMessageRequestDto request)
        {
            var message = await _messageService.SendMessageAsync(GetUserId(), request);
            return CreatedAtAction(nameof(GetMessages), new { chatId = message.ChatId, messageId = message.Id }, message);
        }

        [HttpPut("messages/{messageId}/read")]
        public async Task<IActionResult> MarkMessageAsRead(Guid messageId)
        {
            await _messageService.MarkAsReadAsync(GetUserId(), messageId);
            return NoContent();
        }
    }
}