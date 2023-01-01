using Commons;
using Mango.ShoppingCartApi.Dtos;

namespace Mango.ShoppingCartApi.Services;

public interface IProductService
{
    Task<Result<object?>> CreateOrUpdateProduct(ProductDto product);
}