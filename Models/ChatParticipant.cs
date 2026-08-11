namespace Luna.Models
{
    public class ChatParticipant
    {
        public Guid ChatId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ParticipantRole Role { get; set; } = ParticipantRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public Chat Chat { get; set; } = null!;
        public AppUser User { get; set; } = null!;
    }

    public enum ParticipantRole
    {
        Admin,
        Member
    }
}