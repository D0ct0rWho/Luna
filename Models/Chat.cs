namespace Luna.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }        // для групповых чатов
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}