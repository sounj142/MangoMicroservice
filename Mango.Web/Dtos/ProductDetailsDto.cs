using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Dtos;

public class ProductDetailsDto : ProductDto
{
    [Range(1, 100)]
    public int Count { get; set; } = 1;
}