using AutoMapper;
using Luna.Data;
using Luna.Dtos;
using Luna.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luna.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ChatService(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ChatDto>> GetUserChatsAsync(string userId)
        {
            var chats = await _context.Chats
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .Where(c => c.Participants.Any(p => p.UserId == userId))
                .ToListAsync();
            return _mapper.Map<IEnumerable<ChatDto>>(chats);
        }

        public async Task<ChatDto> CreateChatAsync(string userId, CreateChatRequestDto request)
        {
            var participants = new List<AppUser>();
            foreach (var username in request.ParticipantUsernames)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) throw new KeyNotFoundException($"Пользователь {username} не найден");
                participants.Add(user);
            }
            // если чат не групповой, убедимся, что добавляется ровно один участник
            if (!request.IsGroup && participants.Count != 1)
                throw new ArgumentException("Личный чат должен иметь ровно одного собеседника");

            var chat = new Chat
            {
                Id = Guid.NewGuid(),
                Name = request.IsGroup ? request.Name : null,
                IsGroup = request.IsGroup,
                CreatedAt = DateTime.UtcNow
            };

            // добавляем создателя
            var creator = await _userManager.FindByIdAsync(userId);
            chat.Participants.Add(new ChatParticipant { UserId = userId, Role = ParticipantRole.Admin });
            foreach (var p in participants)
            {
                if (p.Id != userId) // на случай если создатель указал себя
                    chat.Participants.Add(new ChatParticipant { UserId = p.Id, Role = ParticipantRole.Member });
            }

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            // подгружаем данные для маппинга
            await _context.Entry(chat).Collection(c => c.Participants).LoadAsync();
            foreach (var cp in chat.Participants)
                await _context.Entry(cp).Reference(p => p.User).LoadAsync();

            return _mapper.Map<ChatDto>(chat);
        }

        public async Task<ChatDto?> GetChatByIdAsync(Guid chatId)
        {
            var chat = await _context.Chats
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c => c.Id == chatId);
            return chat == null ? null : _mapper.Map<ChatDto>(chat);
        }
    }
}