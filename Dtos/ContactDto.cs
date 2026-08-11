namespace Luna.Dtos
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ContactUserId { get; set; } = string.Empty;
        public string ContactUsername { get; set; } = string.Empty;  // для отображения
        public string Status { get; set; } = string.Empty;
    }
}