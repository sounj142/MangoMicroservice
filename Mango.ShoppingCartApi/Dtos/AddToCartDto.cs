namespace Mango.ShoppingCartApi.Dtos;

public class AddToCartDto
{
    public int Count { get; set; }
    public ProductDto Product { get; set; } = new();
}