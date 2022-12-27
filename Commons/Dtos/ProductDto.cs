namespace Commons.Dtos;

public class ProductDto : ProductCreateDto
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}