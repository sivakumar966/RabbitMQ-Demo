namespace ReportAPI.Data
{
    public class Product
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Ordered { get; set; }
    }
}
