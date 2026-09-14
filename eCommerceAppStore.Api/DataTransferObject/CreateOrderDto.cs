using System.ComponentModel.DataAnnotations;

namespace eCommerceAppStore.Api.DataTransferObject;

/// <summary>
/// Data Transfer Object for creating an order.
/// Adnotari de date -> fortam 400 Bad Request-ul daca datele nu se respecta
/// </summary>
public class CreateOrderDto
{
    [Required(ErrorMessage = "ID-ul produsului este obligatoriu")]
    [Range(1, int.MaxValue, ErrorMessage = "ID-ul produsului trebuie sa fie valid")]
    public int ProductId { get; set; }

    [Range(1, 1000, ErrorMessage = "Cantitatea trebuie sa fie intre 1 si 1000 de bucati")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Email-ul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Formatul email-ului este invalid")]
    public string CustomerEmail { get; set; } = string.Empty;
}