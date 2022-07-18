namespace ProductAPI.Data
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
                                new Product() {ProductName = "Parle-G" , Price = 5 , CreatedOn = DateTime.Now.AddMinutes(10)},
                                new Product() {ProductName = "OREO" , Price = 10, CreatedOn = DateTime.Now.AddMinutes(7)},
                                new Product() {ProductName = "Good Day" , Price = 10, CreatedOn = DateTime.Now.AddMinutes(3)}
                            }
                        );

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
