using Commons.Dtos;
using Commons;

namespace Mango.Web.Services;

public interface IProductService
{
    Task<Result<List<ProductDto>?>> GetProducts();

    Task<Result<ProductDto?>> GetProduct(Guid id);

    Task<Result<ProductDto?>> CreateProduct(ProductCreateDto product);

    Task<Result<ProductDto?>> UpdateProduct(ProductUpdateDto product);

    Task<Result<object?>> DeleteProduct(Guid id);
}