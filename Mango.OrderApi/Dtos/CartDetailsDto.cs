namespace Mango.OrderApi.Dtos;

public class CartDetailsDto
{
    public Guid Id { get; set; }

    public int Count { get; set; }

    public Guid CartHeaderId { get; set; }

    public Guid ProductId { get; set; }

    public ProductDto? Product { get; set; }
}