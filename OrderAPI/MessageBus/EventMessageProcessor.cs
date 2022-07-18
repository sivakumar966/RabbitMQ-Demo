using OrderAPI.Data;
using OrderAPI.DTOs;
using System.Text.Json;

namespace OrderAPI.MessageBus
{
    public class EventMessageProcessor : IEventMessageProcessor
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<EventMessageProcessor> logger;

        public EventMessageProcessor(AppDbContext dbContext, ILogger<EventMessageProcessor> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task ProcessNotification(string Message)
        {
            try
            {
                var genericEvent = JsonSerializer.Deserialize<GenericEventDTO>(Message);
                switch (genericEvent.Event)
                {

                    case "ProductAdded":
                        await AddProduct(Message);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }


        private async Task AddProduct(string Message)
        {
            try
            {
                ProductAddedDTO product = JsonSerializer.Deserialize<ProductAddedDTO>(Message);
                if (!dbContext.Products.Any(x => x.ProductId == product.ProductId))
                {
                    await dbContext.Products.AddAsync(new Product()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }
    }
}
