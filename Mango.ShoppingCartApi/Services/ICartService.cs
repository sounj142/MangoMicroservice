using Commons;
using Mango.ShoppingCartApi.Dtos;
using Mango.ShoppingCartApi.Messages;

namespace Mango.ShoppingCartApi.Services;

public interface ICartService
{
    Task<Result<CartHeaderDto?>> GetOrCreateCartByUserId(string userId);

    Task<Result<CartHeaderDto?>> CreateOrUpdateCart(
        string userId, int count, ProductDto product);

    Task<Result<CartHeaderDto?>> RemoveFromCart(string userId, Guid productId);

    Task<Result<object?>> ClearCart(string userId);

    Task<Result<CartHeaderDto?>> ApplyCoupon(
        string userId, string? couponCode, double discountAmount);

    Task<Result<object?>> Checkout(string userId, CheckoutDto model);
}