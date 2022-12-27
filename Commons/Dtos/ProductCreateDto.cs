namespace Commons.Dtos;

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
}