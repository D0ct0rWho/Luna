using Microsoft.AspNetCore.Identity;

namespace Luna.Models
{
    public class AppUser : IdentityUser
    {
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
        public DateTime LastSeen { get; set; } = DateTime.UtcNow; // LastSeen поможет показывать онлайн-статус

        // Навигационные Коллекции упрощают запросы к связанным данным.
        // навигационные свойства (коллекции) не требуют столбцов, их EF Core обрабатывает через внешние ключи
        // Навигационные свойства
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}