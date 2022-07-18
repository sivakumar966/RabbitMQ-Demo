namespace ReportAPI.Data
{
    public static class DbSeed
    {
        public static async void RunDBSeed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!dbContext.Products.Any())
                {
                    await dbContext.Products.AddRangeAsync(
                            new List<Product>() {
                                new Product() {ProductName = "Parle-G" , Price = 5 , ProductId = 1, Stock = 98, Ordered = 2 },
                                new Product() {ProductName = "OREO" , Price = 10, ProductId = 2, Stock = 99, Ordered = 1 },
                                new Product() {ProductName = "Good Day" , Price = 10, ProductId = 3, Stock = 97, Ordered = 3 }
                            }
                        );

                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Orders.Any())
                {
                    await dbContext.Orders.AddRangeAsync(
                           new List<Order>() {
                                new Order() { OrderId = 1, Product = "Parle-G" ,Quantity = 2, Username="Shiva" },
                                new Order() { OrderId = 2, Product = "OREO" ,Quantity = 1, Username="Kumar" },
                                new Order() { OrderId = 3, Product ="Good Day" ,Quantity = 3, Username="Raj" }
                           }
                       );

                    await dbContext.SaveChangesAsync();
                }

            }
        }
    }
}
