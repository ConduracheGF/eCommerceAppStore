using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eCommerceAppStore.Api.Data;
using eCommerceAppStore.Api.Controllers;
using eCommerceAppStore.Api.DataTransferObject;

namespace eCommerceAppStore.Tests;

public class OrdersControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AllCommands()
    {
        var context = GetInMemoryDbContext();
        context.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 3000, Stock = 5 });
        await context.SaveChangesAsync();

        var controller = new OrdersController(context);
        var orderDto = new CreateOrderDto { ProductId = 1, Quantity = 2, CustomerEmail = "test@test.com" };

        var result = await controller.CreateOrder(orderDto);

        var updatedProduct = await context.Products.FindAsync(1);
        Assert.Equal(3, updatedProduct!.Stock);
        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}
