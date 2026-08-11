using AutoMapper;
using Luna.Data;
using Luna.Dtos;
using Luna.Models;
using Microsoft.EntityFrameworkCore;

namespace Luna.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MessageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid chatId, int page = 1, int pageSize = 50)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<MessageDto> SendMessageAsync(string senderId, SendMessageRequestDto request)
        {
            // Проверяем, что отправитель состоит в чате
            var participant = await _context.ChatParticipants
                .FirstOrDefaultAsync(cp => cp.ChatId == request.ChatId && cp.UserId == senderId);
            if (participant == null)
                throw new UnauthorizedAccessException("Вы не являетесь участником этого чата");

            var message = new Message
            {
                Id = Guid.NewGuid(),
                ChatId = request.ChatId,
                SenderId = senderId,
                Content = request.Content,
                SentAt = DateTime.UtcNow,
                Status = MessageStatus.Sent
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Загружаем отправителя для маппинга
            await _context.Entry(message).Reference(m => m.Sender).LoadAsync();
            return _mapper.Map<MessageDto>(message);
        }

        public async Task MarkAsReadAsync(string userId, Guid messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null) return;
            // Только получатель может пометить как прочитанное (не отправитель)
            if (message.SenderId == userId) return;
            message.Status = MessageStatus.Read;
            await _context.SaveChangesAsync();
        }
    }
}