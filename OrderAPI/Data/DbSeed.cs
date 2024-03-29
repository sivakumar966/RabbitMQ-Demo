﻿namespace OrderAPI.Data
{
    public static class DbSeed
    {
        public static async void RunDBSeed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!dbContext.Orders.Any())
                {
                    await dbContext.Orders.AddRangeAsync(
                            new List<Order>() {
                                new Order() {OrderedDate = DateTime.Now.AddMinutes(-10), Product = "Parle-G" ,Quantity = 2, Username="Shiva" },
                                new Order() {OrderedDate = DateTime.Now.AddMinutes(-5), Product = "OREO" ,Quantity = 1, Username="Kumar" },
                                new Order() {OrderedDate = DateTime.Now.AddMinutes(-2), Product ="Good Day" ,Quantity = 3, Username="Raj"}
                            }
                        );

                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Products.Any())
                {
                    await dbContext.Products.AddRangeAsync(
                            new List<Product>() {
                                new Product() {ProductName = "Parle-G" , Price = 5 , ProductId = 1 },
                                new Product() {ProductName = "OREO" , Price = 10, ProductId = 2},
                                new Product() {ProductName = "Good Day" , Price = 10, ProductId = 3 }
                            }
                        );

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
