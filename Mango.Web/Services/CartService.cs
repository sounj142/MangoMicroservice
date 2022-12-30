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

    public Task<Result<CartHeaderDto?>> GetOrCreateCartOfCurrentUser()
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = _baseUrl,
        });
    }

    public Task<Result<CartHeaderDto?>> CreateOrUpdateCart(CreateOrUpdateCart dto)
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Post,
            Url = _baseUrl,
            Body = dto
        });
    }

    public Task<Result<CartHeaderDto?>> RemoveFromCart(RemoveFromCartDto dto)
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Put,
            Url = _baseUrl,
            Body = dto
        });
    }

    public Task<Result<object?>> ClearCart()
    {
        return Send<object>(new ApiRequest
        {
            Method = HttpMethod.Delete,
            Url = _baseUrl
        });
    }

    public Task<Result<CartHeaderDto?>> ApplyCoupon(ApplyCouponDto dto)
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Post,
            Url = $"{_baseUrl}/apply-coupon",
            Body = dto
        });
    }
}