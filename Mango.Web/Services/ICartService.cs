using Commons;
using Mango.Web.Dtos;

namespace Mango.Web.Services;

public interface ICartService
{
    Task<Result<CartHeaderDto?>> GetOrCreateCartByUserId(string userId);

    Task<Result<CartHeaderDto?>> CreateOrUpdateCart(string userId, int count, ProductDto product);

    Task<Result<CartHeaderDto?>> RemoveFromCart(string userId, Guid productId);

    Task<Result<object?>> ClearCart(string userId);
}