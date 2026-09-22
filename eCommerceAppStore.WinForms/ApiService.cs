using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace eCommerceAppStore.WinForms;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ApiService
{
    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5113/")
    };

    public static void SetJwtToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<ProductDto>> GetProductsAsync(string? search = null)
    {
        try
        {
            string url = "api/products";
            if (!string.IsNullOrWhiteSpace(search))
            {
                url += $"?search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetFromJsonAsync<List<ProductDto>>(url);
            return response ?? new List<ProductDto>();
        }
        catch
        {
            return new List<ProductDto>();
        }
    }

    public async Task<(bool Success, string ErrorMessage)> CreateProductAsync(ProductDto product)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);
            if (response.IsSuccessStatusCode) return (true, string.Empty);

            string err = await response.Content.ReadAsStringAsync();
            return (false, $"Status {(int)response.StatusCode}: {err}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateProductAsync(int id, ProductDto product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
            if (response.IsSuccessStatusCode) return (true, string.Empty);

            string err = await response.Content.ReadAsStringAsync();
            return (false, $"Status {(int)response.StatusCode}: {err}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string ErrorMessage)> DeleteProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            if (response.IsSuccessStatusCode) return (true, string.Empty);

            string err = await response.Content.ReadAsStringAsync();
            return (false, $"Status {(int)response.StatusCode}: {err}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders");
            return response ?? new List<OrderDto>();
        }
        catch
        {
            return new List<OrderDto>();
        }
    }

    public async Task<(bool Success, string ErrorMessage)> CreateOrderAsync(CreateOrderDto order)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", order);
            if (response.IsSuccessStatusCode) return (true, string.Empty);

            string err = await response.Content.ReadAsStringAsync();
            return (false, $"Status {(int)response.StatusCode}: {err}");
        }
        catch (Exception e)
        {
            return (false, e.Message);
        }
    }
}