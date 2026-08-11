namespace Luna.Dtos
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> ParticipantUsernames { get; set; } = new();
    }
}