using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eCommerceAppStore.Api.Models;

namespace eCommerceAppStore.Api.Data;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Numele produsului este obligatoriu")]
    [StringLength(20, ErrorMessage = "Numele produsului nu poate depasi 20 de caractere")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 100000, ErrorMessage = "Pretul trebuie sa fie mai mare decat 0 si mai mic decat 100000")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Range(0, 10000, ErrorMessage = "Stocul nu poate fi negativ")]
    public int Stock { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();
}