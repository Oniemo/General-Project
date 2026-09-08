namespace test2.Models
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Addres { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string TitleOfStatus { get; set; } = string.Empty;
    }
}
