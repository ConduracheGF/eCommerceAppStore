using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eCommerceAppStore.Api.Data;
using eCommerceAppStore.Api.DataTransferObject;

namespace eCommerceAppStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController: ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null) return NotFound($"Produsul cu ID-ul: {dto.ProductId} nu exista!");
        if (product.Stock < dto.Quantity) return BadRequest($"Stoc insuficient! Stoc curent: {product.Stock}");

        product.Stock -= dto.Quantity;
        var order = new Order
        {
            CustomerEmail = dto.CustomerEmail,
            TotalAmount = product.Price * dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }
}