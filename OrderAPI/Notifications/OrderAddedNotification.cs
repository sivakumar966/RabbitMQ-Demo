namespace OrderAPI.Notifications
{
    public class OrderAddedNotification
    {
        public string Event { get; set; }
        public int OrderId { get; set; }      
        public string Product { get; set; }
        public int Quantity { get; set; }
        public string Username { get; set; }
    }
}
