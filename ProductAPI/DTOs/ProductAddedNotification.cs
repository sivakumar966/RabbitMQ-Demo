namespace ProductAPI.DTOs
{
    public class ProductAddedNotification
    {
        public string Event { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
    }
}
