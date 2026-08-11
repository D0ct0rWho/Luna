namespace Luna.Dtos
{
    public class SendMessageRequestDto
    {
        public Guid ChatId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}