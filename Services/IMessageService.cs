using Luna.Dtos;

namespace Luna.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid chatId, int page = 1, int pageSize = 50);
        Task<MessageDto> SendMessageAsync(string senderId, SendMessageRequestDto request);
        Task MarkAsReadAsync(string userId, Guid messageId);
    }
}