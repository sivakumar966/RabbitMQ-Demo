using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.DTOs;
using OrderAPI.MessageBus;
using OrderAPI.Notifications;
using System.Text.Json;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPublisher publisher;

        public OrdersController(AppDbContext context, IPublisher publisher)
        {
            _context = context;
            this.publisher = publisher;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }


        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderDTO orderDto)
        {
            var order = new Order()
            {
                OrderedDate = DateTime.Now,
                Product = orderDto.Product,
                Quantity = orderDto.Quantity,
                Username = orderDto.Username
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderNotification = new OrderAddedNotification()
            {
                Event = "OrderAdded",
                OrderId = order.Id,
                Username = order.Username,
                Product = order.Product,
                Quantity = order.Quantity
            };
            publisher.SendMessage(JsonSerializer.Serialize(orderNotification)); 

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

    }
}
