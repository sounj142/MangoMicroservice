using Commons;
using Mango.ProductAPI.Dtos;

namespace Mango.ProductAPI.Services;

public interface IProductService
{
    Task<Result<List<ProductDto>?>> GetProducts();

    Task<Result<ProductDto?>> GetProduct(Guid id);

    Task<Result<ProductDto?>> CreateProduct(ProductCreateDto product);

    Task<Result<ProductDto?>> UpdateProduct(Guid id, ProductCreateDto product);

    Task<Result<object?>> DeleteProduct(Guid id);
}