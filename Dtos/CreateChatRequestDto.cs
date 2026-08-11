namespace Luna.Dtos
{
    public class CreateChatRequestDto
    {
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public List<string> ParticipantUsernames { get; set; } = new();
    }
}