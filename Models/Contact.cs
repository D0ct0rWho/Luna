namespace Luna.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;          // кто отправил запрос
        public string ContactUserId { get; set; } = string.Empty;   // кому отправили
        public ContactStatus Status { get; set; } = ContactStatus.Pending;

        // Навигационные свойства
        public AppUser User { get; set; } = null!;
        public AppUser ContactUser { get; set; } = null!;
    }

    public enum ContactStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
