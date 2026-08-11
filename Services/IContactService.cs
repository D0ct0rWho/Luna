using Luna.Dtos;

namespace Luna.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetContactsAsync(string userId);
        Task<ContactDto> AddContactAsync(string userId, CreateContactRequestDto request);
        Task AcceptContactAsync(string userId, int contactId);
        Task RejectContactAsync(string userId, int contactId);
        Task RemoveContactAsync(string userId, int contactId);
    }
}