namespace Mango.ShoppingCartApi.Dtos;

public class AddToCartDto
{
    public string UserId { get; set; } = string.Empty;
    public int Count { get; set; }
    public ProductDto Product { get; set; } = new();
}