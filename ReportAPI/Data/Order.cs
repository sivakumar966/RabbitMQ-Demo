namespace ReportAPI.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public string Username { get; set; }
    }
}
