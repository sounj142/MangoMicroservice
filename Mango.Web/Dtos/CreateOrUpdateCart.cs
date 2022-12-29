namespace Mango.Web.Dtos;

public class CreateOrUpdateCart
{
    public string UserId { get; set; } = string.Empty;
    public int Count { get; set; }
    public ProductDto Product { get; set; } = new();
}