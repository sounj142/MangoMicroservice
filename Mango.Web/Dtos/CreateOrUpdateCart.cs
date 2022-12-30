namespace Mango.Web.Dtos;

public class CreateOrUpdateCart
{
    public int Count { get; set; }
    public ProductDto Product { get; set; } = new();
}