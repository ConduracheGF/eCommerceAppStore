namespace eCommerceAppStore.Api.DataTransferObject;

public class CreateOrderDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
}