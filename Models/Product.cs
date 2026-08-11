namespace Luna.Models
{
    public class Product
    {
        public int Id { get; set; }          // первичный ключ
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
