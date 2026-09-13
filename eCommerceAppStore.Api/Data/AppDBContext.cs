using Microsoft.EntityFrameworkCore;

namespace eCommerceAppStore.Api.Data;

public class Product
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Stock { get; set; }
}

public class Order
{
	public int Id { get; set; }
	public string CustomerEmail { get; set; } = string.Empty;
	public decimal TotalAmount { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Product> Products => Set<Product>();
	public DbSet<Order> Orders => Set<Order>();
}