using Luna.Dtos;

namespace Luna.Services
{
    public interface IChatService
    {
        Task<IEnumerable<ChatDto>> GetUserChatsAsync(string userId);
        Task<ChatDto> CreateChatAsync(string userId, CreateChatRequestDto request);
        Task<ChatDto?> GetChatByIdAsync(Guid chatId);
    }
}