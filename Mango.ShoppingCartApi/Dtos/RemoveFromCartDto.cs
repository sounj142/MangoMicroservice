namespace Mango.ShoppingCartApi.Dtos;

public class RemoveFromCartDto
{
    public string UserId { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
}