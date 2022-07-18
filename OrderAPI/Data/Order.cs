namespace OrderAPI.Data
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderedDate { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public string Username { get; set; }
    }
}
