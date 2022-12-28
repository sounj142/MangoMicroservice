using Commons;
using Mango.Web.Dtos;

namespace Mango.Web.Services;

public interface IProductService
{
    Task<Result<List<ProductDto>?>> GetProducts();

    Task<Result<ProductDto?>> GetProduct(string id);

    Task<Result<ProductDto?>> CreateProduct(ProductCreateDto product);

    Task<Result<ProductDto?>> UpdateProduct(ProductUpdateDto product);

    Task<Result<object?>> DeleteProduct(string id);
}