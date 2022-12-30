using Commons;
using Mango.Web.Dtos;
using Mango.Web.Models;

namespace Mango.Web.Services;

public class CartService : BaseService, ICartService
{
    private readonly string _baseUrl;

    public CartService(
        ServiceUrls serviceUrls,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BaseService> logger)
        : base(httpClientFactory, httpContextAccessor, logger)
    {
        _baseUrl = $"{serviceUrls.ShoppingCartApi}/api/v1/cart";
    }

    public async Task<Result<CartHeaderDto?>> GetOrCreateCartByUserId(string userId)
    {
        var result = await Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = _baseUrl,
            Params = new { userId }
        });
        CalculateCartTotalPrice(result.Data);
        return result;
    }

    public async Task<Result<CartHeaderDto?>> CreateOrUpdateCart(string userId, int count, ProductDto product)
    {
        var result = await Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Post,
            Url = _baseUrl,
            Body = new CreateOrUpdateCart
            {
                UserId = userId,
                Count = count,
                Product = product
            }
        });
        CalculateCartTotalPrice(result.Data);
        return result;
    }

    public async Task<Result<CartHeaderDto?>> RemoveFromCart(string userId, Guid productId)
    {
        var result = await Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Put,
            Url = _baseUrl,
            Body = new RemoveFromCartDto
            {
                UserId = userId,
                ProductId = productId
            }
        });
        CalculateCartTotalPrice(result.Data);
        return result;
    }

    public Task<Result<object?>> ClearCart(string userId)
    {
        return Send<object>(new ApiRequest
        {
            Method = HttpMethod.Delete,
            Url = _baseUrl,
            Params = new { userId }
        });
    }

    private void CalculateCartTotalPrice(CartHeaderDto? cart)
    {
        if (cart == null) return;
        cart.TotalPrice = cart.CartDetails.Sum(item => item.Count * item.Product!.Price);
        cart.FinalPrice = cart.TotalPrice;
    }
}