namespace Luna.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public MessageStatus Status { get; set; } = MessageStatus.Sent;
        public string? AttachmentUrl { get; set; }

        public Chat Chat { get; set; } = null!;
        public AppUser Sender { get; set; } = null!;
    }

    public enum MessageStatus
    {
        Sent,
        Delivered,
        Read
    }
}