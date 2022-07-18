using Microsoft.EntityFrameworkCore;
using ReportAPI.Data;
using ReportAPI.DTOs;
using System.Text.Json;

namespace ReportAPI.MessageBus
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
                    case "OrderAdded":
                        await AddOrder(Message);
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
                        Price = product.Price,
                        Ordered = 0,
                        Stock = 100
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        private async Task AddOrder(string Message)
        {
            try
            {
                OrderAddedDTO orderAdded = JsonSerializer.Deserialize<OrderAddedDTO>(Message);

                var product = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductName == orderAdded.Product);

                if (product == null)
                {
                    await dbContext.Products.AddAsync(new Product()
                    {
                        ProductId = 0,
                        ProductName = orderAdded.Product,
                        Price = 0,
                        Ordered = orderAdded.Quantity,
                        Stock = (100 - orderAdded.Quantity)
                    });
                }
                else
                {
                    product.Ordered = product.Ordered + orderAdded.Quantity;
                    product.Stock = product.Stock - orderAdded.Quantity;
                }

                if (!dbContext.Orders.Any(x => x.OrderId == orderAdded.OrderId))
                {
                    dbContext.Orders.Add(new Order()
                    {
                        OrderId = orderAdded.OrderId,
                        Product = orderAdded.Product,
                        Quantity = orderAdded.Quantity,
                        Username = orderAdded.Username
                    });
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }
    }
}
