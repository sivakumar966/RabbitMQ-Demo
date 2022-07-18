namespace ReportAPI.DTOs
{
    public class OrderAddedDTO
    {
        public int OrderId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public string Username { get; set; }
    }
}
