using Commons;
using Mango.Web.Dtos;

namespace Mango.Web.Services;

public interface ICartService
{
    Task<Result<CartHeaderDto?>> GetOrCreateCartOfCurrentUser();

    Task<Result<CartHeaderDto?>> CreateOrUpdateCart(CreateOrUpdateCart dto);

    Task<Result<CartHeaderDto?>> RemoveFromCart(RemoveFromCartDto dto);

    Task<Result<object?>> ClearCart();

    Task<Result<CartHeaderDto?>> ApplyCoupon(ApplyCouponDto dto);
}