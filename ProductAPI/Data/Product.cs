﻿namespace ProductAPI.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
