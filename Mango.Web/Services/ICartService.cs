using Commons;
using Mango.Web.Dtos;

namespace Mango.Web.Services;

public interface ICartService
{
    Task<Result<CartHeaderDto?>> GetOrCreateCartByUserId(string userId);

    Task<Result<CartHeaderDto?>> CreateOrUpdateCart(CreateOrUpdateCart dto);

    Task<Result<CartHeaderDto?>> RemoveFromCart(RemoveFromCartDto dto);

    Task<Result<object?>> ClearCart(string userId);

    Task<Result<CartHeaderDto?>> ApplyCoupon(ApplyCouponDto dto);
}