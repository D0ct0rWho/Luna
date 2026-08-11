namespace Luna.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
    }
}