using AutoMapper;
using Luna.Data;
using Luna.Dtos;
using Luna.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luna.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ContactService(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ContactDto>> GetContactsAsync(string userId)
        {
            var contacts = await _context.Contacts
                .Include(c => c.ContactUser)
                .Where(c => c.UserId == userId || c.ContactUserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ContactDto>>(contacts);
        }

        public async Task<ContactDto> AddContactAsync(string userId, CreateContactRequestDto request)
        {
            var contactUser = await _userManager.FindByNameAsync(request.ContactUsername);
            if (contactUser == null)
                throw new KeyNotFoundException("Пользователь не найден");

            var existing = await _context.Contacts.FirstOrDefaultAsync(
                c => (c.UserId == userId && c.ContactUserId == contactUser.Id) ||
                     (c.UserId == contactUser.Id && c.ContactUserId == userId));
            if (existing != null)
                throw new InvalidOperationException("Контакт уже существует");

            var contact = new Contact
            {
                UserId = userId,
                ContactUserId = contactUser.Id,
                Status = ContactStatus.Pending
            };
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // подгружаем ContactUser для маппинга
            await _context.Entry(contact).Reference(c => c.ContactUser).LoadAsync();
            return _mapper.Map<ContactDto>(contact);
        }

        public async Task AcceptContactAsync(string userId, int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null || contact.ContactUserId != userId)
                throw new KeyNotFoundException("Запрос не найден");

            contact.Status = ContactStatus.Accepted;
            await _context.SaveChangesAsync();
        }

        public async Task RejectContactAsync(string userId, int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null || contact.ContactUserId != userId)
                throw new KeyNotFoundException("Запрос не найден");

            contact.Status = ContactStatus.Rejected;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveContactAsync(string userId, int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null || (contact.UserId != userId && contact.ContactUserId != userId))
                throw new KeyNotFoundException("Контакт не найден");

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}