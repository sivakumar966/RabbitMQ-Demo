using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Data;

namespace ReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ReportController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await dbContext.Products.Select(x => new { x.ProductId, x.ProductName, x.Price, x.Ordered, x.Stock }).ToListAsync();
            return Ok(products);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await dbContext.Orders.Select(x => new { x.OrderId, x.Product, x.Quantity, x.Username }).ToListAsync();
            return Ok(orders);
        }

    }
}
