using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs;
using ProductAPI.MessageBus;
using System.Text.Json;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IPublisher publisher;


        public ProductController(AppDbContext dbContext, IPublisher publisher)
        {
            this.dbContext = dbContext;
            this.publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await dbContext.Products.ToListAsync();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProductDtos productDto)
        {
            var product = new Product()
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                CreatedOn = DateTime.Now
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            ProductAddedNotification productCreatedNotification = new ProductAddedNotification()
            {
                Event = "ProductAdded",
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price
            };

            publisher.SendMessage(JsonSerializer.Serialize(productCreatedNotification));
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);           
        }


    }
}
